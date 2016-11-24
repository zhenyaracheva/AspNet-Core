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
                    Description ="Home Town",
                     PointsOfInterest = new List<PointsOfInterestDto>()
                     {
                          new PointsOfInterestDto { Id=1, Name="Moskva", Description="The Bar" },
                          new PointsOfInterestDto { Id=2, Name="Krepostta", Description="The Place" }
                     }
                },
                new CityDto()
                {
                    Id =2,
                    Name ="Sofia",
                    Description ="Live Town",
                     PointsOfInterest = new List<PointsOfInterestDto>()
                     {
                          new PointsOfInterestDto { Id=1, Name="Switch", Description="The Bar" },
                          new PointsOfInterestDto { Id=2, Name="Vitosha", Description="The Place" }
                     }
                }
            };
        }
    }
}
