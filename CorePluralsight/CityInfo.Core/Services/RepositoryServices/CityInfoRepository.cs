namespace CityInfo.Core.Services.RepositoryServices
{
    using System;
    using System.Collections.Generic;
    using CityInfo.Core.Entities;
    using Entities.DbContexts;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoDbContext context;

        public CityInfoRepository(CityInfoDbContext cityContext)
        {
            context = cityContext;
        }

        public bool CityExist(int cityId)
        {
            return context.Cities.Any(x => x.Id == cityId);
        }

        public IEnumerable<City> GetCities()
        {
            return context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterests)
        {
            if (includePointsOfInterests)
            {
                return context.Cities
                    .Include(c => c.PointsOfInterest)
                    .FirstOrDefault(x => x.Id == cityId);
            }

            return context.Cities.FirstOrDefault(c => c.Id == cityId);
        }

        public PointOfInterest GetPointOfInterestFromCity(int cityId, int pointOfIntrestId)
        {
            return context.PointsOfInterest
                .FirstOrDefault(p => p.Id == pointOfIntrestId && p.CityId == cityId);
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return context.PointsOfInterest
                .Where(x => x.CityId == cityId)
                .ToList();
        }
    }
}
