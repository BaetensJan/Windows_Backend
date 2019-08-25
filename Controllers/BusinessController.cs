using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows_Backend.DTO;
using Windows_Backend.Entities;
using Windows_Backend.Interfaces;
using IronPdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBarcode;

namespace Windows_Backend.Controllers
{
    [Route("[controller]/[action]")]
    public class BusinessController : Controller
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IPromotionRepository _promotionRepository;

        public BusinessController(IBusinessRepository businessRepository, UserManager<User> userManager,
            IUserRepository userRepository, IEventRepository eventRepository,
            IPromotionRepository promotionRepository)
        {
            _businessRepository = businessRepository;
            _userManager = userManager;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _promotionRepository = promotionRepository;
        }

        [HttpGet]
        public async Task<List<Business>> Index()
        {
            var b = await _businessRepository.All();
            return b;
        }

        [HttpGet("{id}")]
        public async Task<BusinessDTO> Index([FromRoute] int id)
        {
            var result = await _businessRepository.FindById(id);
            BusinessDTO businessDto = new BusinessDTO();
            businessDto.Name = result.Name;
            businessDto.Type = result.Type;
            businessDto.Address = result.Address;
            List<EventDTO> eventDtos = new List<EventDTO>();
            foreach (var eventDto in result.Events)
            {
                eventDtos.Add(new EventDTO
                {
                    Id = eventDto.Id, Name = eventDto.Name, Creation = eventDto.Creation,
                    Description = eventDto.Description, Type = eventDto.Type
                });
            }

            List<PromotionDTO> promotionsDtos = new List<PromotionDTO>();
            foreach (var promotionDto in result.Promotions)
            {
                promotionsDtos.Add(new PromotionDTO
                {
                    Id = promotionDto.Id, Name = promotionDto.Name, Creation = promotionDto.Creation,
                    PromotionType = promotionDto.PromotionType, Description = promotionDto.Description,
                    EndDate = promotionDto.EndDate,
                    StartDate = promotionDto.StartDate
                });
            }

            businessDto.Events = eventDtos;
            businessDto.Promotions = promotionsDtos;
            return businessDto;
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

            await _businessRepository.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddPromotion([FromBody] BusinessDTO model)
        {
            var email = (await _userManager.GetUserAsync(HttpContext.User)).Email;
            var user = await _userRepository.FindByEmail(email);

            var business = user.Business;
            if (business == null) return Unauthorized();

            if (business.Promotions != null)
            {
                await _promotionRepository.RemoveMultiple(business.Promotions);
            }

            business.Promotions = new List<Promotion>();
            await _businessRepository.SaveChanges();
            foreach (var promotion in model.Promotions)
            {
                business.Promotions.Add(new Promotion()
                {
                    Name = promotion.Name,
                    PromotionType = promotion.PromotionType,
                    StartDate = promotion.StartDate,
                    EndDate = promotion.EndDate,
                    Description = promotion.Description,
                    Creation = promotion.Creation
                });
            }

            await _businessRepository.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddEvents([FromBody] BusinessDTO model)
        {
            var email = (await _userManager.GetUserAsync(HttpContext.User)).Email;
            var user = await _userRepository.FindByEmail(email);

            var business = user.Business;
            if (business == null) return Unauthorized();

            if (business.Events != null)
            {
                await _eventRepository.RemoveMultiple(business.Events);
            }

            business.Events = new List<Event>();
            await _businessRepository.SaveChanges();

            foreach (var ev in model.Events)
            {
                business.Events.Add(new Event()
                {
                    Name = ev.Name,
                    Description = ev.Description,
                    Type = ev.Type,
                    Creation = ev.Creation
                });
            }

            await _businessRepository.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PdfForPromotion([FromRoute] int id)
        {
            //TODO: add validation if promotion has pdf coupon
            var renderer = new HtmlToPdf();
            var myBarCode = new Barcode($"Promotion_{id}");
            var html = $"<img src=\"data:image/png;base64, {myBarCode.GetBase64Image()}\"/>";
            var memory = renderer.RenderHtmlAsPdf(html).Stream;

            memory.Position = 0;
            return new FileStreamResult(memory, "application/pdf");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveEvents([FromBody] EventDTO model)
        {
            var email = (await _userManager.GetUserAsync(HttpContext.User)).Email;
            var user = await _userRepository.FindByEmail(email);

            var business = user.Business;
            if (business == null) return Unauthorized();

            if (business.Events != null)
            {
                await _eventRepository.RemoveEvent(new Event
                    {Id = model.Id, Description = model.Description, Name = model.Name, Type = model.Type});
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemovePromotion([FromBody] PromotionDTO model)
        {
            var email = (await _userManager.GetUserAsync(HttpContext.User)).Email;
            var user = await _userRepository.FindByEmail(email);

            var business = user.Business;
            if (business == null) return Unauthorized();

            if (business.Promotions != null)
            {
                await _promotionRepository.RemovePromotion(new Promotion
                {
                    Id = model.Id, Description = model.Description, Name = model.Name,
                    PromotionType = model.PromotionType, StartDate = model.StartDate,
                    EndDate = model.EndDate
                });
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditPromotion([FromBody] PromotionDTO model)
        {
            var email = (await _userManager.GetUserAsync(HttpContext.User)).Email;
            var user = await _userRepository.FindByEmail(email);

            var business = user.Business;
            if (business == null) return Unauthorized();

            if (business.Promotions == null)
            {
                new Exception("Promotielijst is leeg, er kan niets worden aangepast.");
            }

            var editPromotion = await _promotionRepository.FindById(model.Id);
            editPromotion.Name = model.Name;
            editPromotion.Description = model.Description;
            editPromotion.PromotionType = model.PromotionType;
            editPromotion.EndDate = model.EndDate;
            editPromotion.StartDate = model.StartDate;

            await _promotionRepository.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent([FromBody] EventDTO model)
        {
            var email = (await _userManager.GetUserAsync(HttpContext.User)).Email;
            var user = await _userRepository.FindByEmail(email);

            var business = user.Business;
            if (business == null) return Unauthorized();

            if (business.Promotions == null)
            {
                new Exception("Eventlijst is leeg, er kan niets worden aangepast.");
            }

            var editEvent = await _eventRepository.FindEventById(model.Id);
            editEvent.Name = model.Name;
            editEvent.Description = model.Description;
            editEvent.Type = model.Type;

            await _promotionRepository.SaveChanges();

            return Ok();
        }

        private void CreateNotification()
        {
        }
    }
}