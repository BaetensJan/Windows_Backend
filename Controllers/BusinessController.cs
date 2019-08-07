using System.Collections.Generic;
using System.Threading.Tasks;
using Windows_Backend.DTO;
using Windows_Backend.Entities;
using Windows_Backend.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Windows_Backend.Controllers
{
    [Route("[controller]/[action]")]
    public class BusinessController : Controller
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        public BusinessController(IBusinessRepository businessRepository, UserManager<User> userManager,
            IUserRepository userRepository, IEventRepository eventRepository)
        {
            _businessRepository = businessRepository;
            _userManager = userManager;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public async Task<List<Business>> Index()
        {
            var b = await _businessRepository.All();
            return b;
        }

        [HttpGet("{id}")]
        public async Task<Business> Index([FromRoute] int id)
        {
            return await _businessRepository.FindById(id);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] BusinessDTO model)
        {
            var email = (await _userManager.GetUserAsync(HttpContext.User)).Email;
            var user = await _userRepository.FindByEmail(email);

            var business = user.Business;
            if (business == null) return Unauthorized();
            business.Name = model.Name;
            business.Type = model.Type;
            business.Address = model.Address;

            _eventRepository.RemoveMultiple(business.Events);

            business.Events = new List<Event>();
            await _businessRepository.SaveChanges();

            foreach (var ev in model.Events)
            {
                business.Events.Add(new Event()
                {
                    Name = ev.Name,
                    Description = ev.Description,
                    Type = ev.Type
                });
            }

            await _businessRepository.SaveChanges();
            return Ok();
        }
    }
}