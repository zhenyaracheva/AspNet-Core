namespace CityInfo.Core.Controllers
{
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
            var citiesResult = new List<CityInfoWithoutPointsOfInterest>();

            foreach (var city in citiesFromDb)
            {
                var current = new CityInfoWithoutPointsOfInterest()
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                };

                citiesResult.Add(current);
            }

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
                var result = new CityDto
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                };

                foreach (var pointOfInterest in city.PointsOfInterest)
                {
                    result.PointsOfInterest.Add(new PointsOfInterestDto
                    {
                        Id = pointOfInterest.Id,
                        Name = pointOfInterest.Name,
                        Description = pointOfInterest.Description
                    });
                }

                return Ok(result);
            }

            var cityResult = new CityInfoWithoutPointsOfInterest
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            };
            return Ok(cityResult);
        }
    }
}
