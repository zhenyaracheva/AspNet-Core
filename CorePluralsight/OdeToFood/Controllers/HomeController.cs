namespace OdeToFood.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using OdeToFood.Services.Interfaces;

    public class HomeController : Controller
    {
        public IRestaurantService restaurantService { get; set; }

        public HomeController( IRestaurantService restartantData)
        {
            restaurantService = restartantData;
        }

        [Route("home/hello")]
        public IActionResult Hello()
        {
            var restourants = restaurantService.GetAll();

            return Ok(restourants);
        }
    }
}
