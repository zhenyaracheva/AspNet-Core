namespace CityInfo.Core.Services.RepositoryServices
{
    using CityInfo.Core.Entities;
    using System.Collections.Generic;

    public interface ICityInfoRepository
    {
        bool CityExist(int cityId);

        IEnumerable<City> GetCities();

        City GetCity(int cityID, bool includePointsOfInteres);

        IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);

        PointOfInterest GetPointOfInterestFromCity(int cityId, int pointOfIntrestId);

        bool AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        bool DeletePointofInterest(PointOfInterest pointOfInterest);

        bool Save();
    }
}
