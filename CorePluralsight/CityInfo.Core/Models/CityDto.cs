namespace CityInfo.Core.Models
{
    using System.Collections.Generic;

    public class CityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int NumberOfPointsOfInterest
        {
            get
            {
                return this.PointsOfInterest.Count;
            }
        }

        public ICollection<PointsOfInterestDto> PointsOfInterest { get; set; } = new List<PointsOfInterestDto>();
    }
}
