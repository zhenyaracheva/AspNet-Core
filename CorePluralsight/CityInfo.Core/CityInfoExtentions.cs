using CityInfo.Core.Entities;
using CityInfo.Core.Entities.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Core
{
    public static class CityInfoExtentions
    {
        public static void EnsureSeedDataForContext(this CityInfoDbContext context)
        {
            if(context.Cities.Any())
            {
                return;
            }

            var cities = new List<City>()
            {
                new City()
                {
                    Name ="Silistra",
                    Description ="Home Town",
                     PointsOfInterest = new List<PointOfInterest>()
                     {
                          new PointOfInterest { Name="Moskva", Description="The Bar" },
                          new PointOfInterest { Name="Krepostta", Description="The Place" }
                     }
                },
                new City()
                {
                    Name ="Sofia",
                    Description ="Live Town",
                     PointsOfInterest = new List<PointOfInterest>()
                     {
                          new PointOfInterest {  Name="Switch", Description="The Bar" },
                          new PointOfInterest {  Name="Vitosha", Description="The Place" }
                     }
                }                
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
