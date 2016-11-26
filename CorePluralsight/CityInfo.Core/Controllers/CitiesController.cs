namespace CityInfo.Core.Controllers
{
    using AutoMapper;
    using DataStore;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services.RepositoryServices;
    using System.Collections.Generic;
    using System.Linq;

    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository cityInfo;

        public CitiesController(ICityInfoRepository cityRepo)
        {
            cityInfo = cityRepo;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            var citiesFromDb = cityInfo.GetCities();
            var citiesResult = Mapper.Map<IEnumerable<CityInfoWithoutPointsOfInterest>>(citiesFromDb);

            return Ok(citiesResult);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = cityInfo.GetCity(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var result = Mapper.Map<CityDto>(city);                
                return Ok(result);
            }

            var cityResult = Mapper.Map<CityInfoWithoutPointsOfInterest>(city);
            return Ok(cityResult);
        }
    }
}
