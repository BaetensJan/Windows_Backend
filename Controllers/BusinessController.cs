using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows_Backend.DTO;
using Windows_Backend.Entities;
using Windows_Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Windows_Backend.Controllers
{
    [Route("[controller]/[action]")]
    public class BusinessController: Controller
    {
        private IBusinessRepository _businessRepository;

        public BusinessController(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }
        
        [HttpGet()]
        public async Task<List<Business>> Index()
        {
            return await _businessRepository.All();
        }
    }
}