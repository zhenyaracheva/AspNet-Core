namespace CityInfo.Core.DataStore
{
    using CityInfo.Core.Models;
    using System.Collections.Generic;

    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public IList<CityDto> Cities;

        public CitiesDataStore()
        {
            this.Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name ="Silistra",
                    Description ="Home Town"
                },
                new CityDto()
                {
                    Id =2,
                    Name ="Sofia",
                    Description ="Live Town"
                }
            };
        }
    }
}
