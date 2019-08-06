using Windows_Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Windows_Backend.Controllers
{
    [Route("[controller]/[action]")]
    public class EventController
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IEventRepository _eventRepository;

        public EventController(IBusinessRepository businessRepository, IEventRepository eventRepository)
        {
            _businessRepository = businessRepository;
            _eventRepository = eventRepository;
        }
    }
}