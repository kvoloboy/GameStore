﻿using GameStore.SeedingServices.Services.Interfaces;
 using Microsoft.AspNetCore.Mvc;

namespace GameStore.Seed.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        private readonly IMongoProductKeyGenerator _mongoProductKeyGenerator;

        public HomeController(IMongoProductKeyGenerator mongoProductKeyGenerator)
        {
            _mongoProductKeyGenerator = mongoProductKeyGenerator;
        }

        [HttpGet]
        public IActionResult Initialize()
        {
            _mongoProductKeyGenerator.SetupKeys();
            return Ok();
        }
    }
}