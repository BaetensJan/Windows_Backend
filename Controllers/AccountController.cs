using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Windows_Backend.DTO;
using Windows_Backend.Entities;
using Windows_Backend.Enums;
using Windows_Backend.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Windows_Backend.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserBusinessRepository _userBusinessRepository;
        private readonly IBusinessRepository _businessRepository;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            IUserRepository userRepository,
            IUserBusinessRepository userBusinessRepository,
            IBusinessRepository businessRepository
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userRepository = userRepository;
            _userBusinessRepository = userBusinessRepository;
            _businessRepository = businessRepository;
        }

        [HttpPost]
        public async Task<object> Login([FromBody] LoginDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                appUser.LastLogin = DateTime.UtcNow;
                _userRepository.SaveChanges();
                return await GenerateJwtToken(model.Email, appUser);
            }
            else
            {
                throw new Exception("Wrong email or password");
            }


            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }
        [HttpGet("{email}")]
        public async Task<List<PromotionDTO>> GetPromotionsFromAbonnees([FromRoute] string email)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == email);
            var lastLogin = appUser.LastLogin;
            var userBusiness = await _userBusinessRepository.FindByUserId(appUser.Id);
            var allBusinesses = await _businessRepository.All();
            List<PromotionDTO> promotions = new List<PromotionDTO>();
            foreach (var ub in userBusiness)
            {
                foreach (var ab in allBusinesses)
                {
                    if (ub.BusinessId == ab.Id)
                    {
                        foreach (var promotion in ab.Promotions)
                        {
                            if (promotion.Creation >= lastLogin)
                            {
                                promotions.Add(new PromotionDTO { Name = promotion.Name, Description = promotion.Description, PromotionType = promotion.PromotionType, StartDate = promotion.ConvertStringToDateTimeOffset(promotion.StartDate) , EndDate = promotion.ConvertStringToDateTimeOffset(promotion.EndDate) });
                            }
                        }
                    }
                }
            }
            return promotions;
        }
        [HttpGet("{email}")]
        public async Task<List<EventDTO>> GetEventsFromAbbonees([FromRoute] string email)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == email);
            var lastLogin = appUser.LastLogin;
            var userBusiness = await _userBusinessRepository.FindByUserId(appUser.Id);
            var allBusinesses = await _businessRepository.All();
            List<EventDTO> events = new List<EventDTO>();
            foreach (var ub in userBusiness)
            {
                foreach (var ab in allBusinesses)
                {
                    if (ub.BusinessId == ab.Id)
                    {
                        foreach (var xEvent in ab.Events)
                        {
                            if (xEvent.Creation >= lastLogin)
                            {
                                events.Add(new EventDTO { Name = xEvent.Name, Description = xEvent.Description, Type = xEvent.Type });
                            }
                        }
                    }
                }
            }
            return events;
        }
        [HttpPost]
        public async Task<object> Register([FromBody] RegisterDTO model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                UserType = model.UserType,
                LastLogin = DateTime.UtcNow
            };
            if (model.UserType == UserType.Business)
            {
                user.Business = new Business()
                {
                    Name = model.Business.Name,
                    Address = model.Business.Address,
                    Type = model.Business.Type
                };
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            var result1 =
                await _userManager.AddToRoleAsync(user, user.UserType == UserType.Business ? "Business" : "Customer");
            _userRepository.SaveChanges();
            if (result.Succeeded && result1.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return await GenerateJwtToken(model.Email, user);
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }

        private async Task<object> GenerateJwtToken(string email, User user)
        {
            var business = (await _userRepository.FindByEmail(email)).Business;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            if (business != null)
            {
                claims.Add(new Claim("businessId", business.Id.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        public async Task<bool> Subscribe([FromBody] int Id)
        {
            var email = (await _userManager.GetUserAsync(HttpContext.User)).Email;
            var user = await _userRepository.FindByEmail(email);
            var alreadySubscribed = true;
            var userBusiness = new UserBusiness
            {
                UserId = user.Id,
                BusinessId = Id,
                
            };
            var count = 0;
            foreach (var subscriber in user.Subscribers)
            {
                count++;
                if (subscriber.UserId == user.Id && subscriber.BusinessId == Id)
                {
                    alreadySubscribed = false;
                    userBusiness.Id = subscriber.Id;
                    user.Subscribers.Last();
                }
            }
            if (alreadySubscribed)
            {
                user.Subscribers.Add(userBusiness);
            }
            else
            {
                _userRepository.RemoveUserBusinessForUserAsync(user.Email, count - 1);
                //user.Subscribers.RemoveAt(count - 1);
            }
            _userRepository.SaveChanges();
            return alreadySubscribed;
        }

        [HttpPost]
        public async Task<bool> CheckUserBusinessForSubscribtion([FromBody] int Id)
        {
            var email = (await _userManager.GetUserAsync(HttpContext.User)).Email;
            var user = await _userRepository.FindByEmail(email);
            var alreadySubscribed = false;
            foreach (var subscriber in user.Subscribers)
            {
                if (subscriber.UserId == user.Id && subscriber.BusinessId == Id)
                {
                    alreadySubscribed = true;
                }
            }
          
            return alreadySubscribed;
        }
    }
}