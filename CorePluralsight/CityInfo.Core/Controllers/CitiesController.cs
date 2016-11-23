namespace CityInfo.Core.Controllers
{
    using DataStore;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    [Route("api/cities")]
    public class CitiesController : Controller
    {
        [HttpGet()]
        public IActionResult GetCities()
        {
            return Ok(new JsonResult(CitiesDataStore.Current.Cities));

        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(x => x.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(new JsonResult(city));
        }
    }
}
