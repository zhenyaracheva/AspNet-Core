namespace OdeToFood.Services
{
    using System;
    using System.Collections.Generic;
    using OdeToFood.Entities;
    using OdeToFood.Services.Interfaces;
    using System.Linq;

    public class RestaurantService : IRestaurantService
    {
        private OdeToFoodDbContext _restaurantsData;

        public RestaurantService(OdeToFoodDbContext odeToFoodRepo)
        {
            this._restaurantsData = odeToFoodRepo;
            //    new List<Restaurant>()
            //{
            //    new Restaurant { Id=1, Name="Mr. Pizza"},
            //    new Restaurant { Id=2, Name="Domino's" },
            //    new Restaurant { Id=3, Name="The Restaurant" }
            //};
        }

        public Restaurant Add(Restaurant restorantToAdd)
        {
            _restaurantsData.Restaurants.Add(restorantToAdd);
            _restaurantsData.SaveChanges();
            return restorantToAdd;
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _restaurantsData.Restaurants;
        }

        public Restaurant GetById(int id)
        {
            return _restaurantsData.Restaurants
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
