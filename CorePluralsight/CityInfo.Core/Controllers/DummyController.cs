using CityInfo.Core.Entities.DbContexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Core.Controllers
{
    public class DummyController: Controller
    {
        private CityInfoDbContext _context;
        
        public DummyController(CityInfoDbContext context)
        {
            _context = context;
        } 

        [HttpGet]
        [Route("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
