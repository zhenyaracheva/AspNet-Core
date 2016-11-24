namespace CityInfo.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PointOfInterestForCreationDto
    {
        [Required(ErrorMessage ="Name is required field")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
