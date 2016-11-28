namespace OdeToFood.Services.Interfaces
{
    using Entities;
    using System.Collections.Generic;

    public interface IRestaurantService
    {
        IEnumerable<Restaurant> GetAll();

        Restaurant GetById(int id);

        Restaurant Add(Restaurant restorantToAdd);
    }
}
