namespace CityInfo.Core.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Microsoft.AspNetCore.JsonPatch;
    using DataStore;
    using Microsoft.Extensions.Logging;

    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> logger;
        private IMailService mailService;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService)
        {
            this.logger = logger;
            this.mailService = mailService;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = DataStore.CitiesDataStore.Current.Cities
                .FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                this.logger.LogInformation($"No city with {cityId} id!");
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointsOfInterest")]
        public IActionResult GetPointsOfInterest(int cityId, int id)
        {
            var city = DataStore.CitiesDataStore.Current.Cities
               .FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest
                .FirstOrDefault(x => x.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.TryAddModelError("Description", "Name and Description cannot be equal");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            var city = DataStore.CitiesDataStore.Current.Cities
                .FirstOrDefault(p => p.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var maxId = DataStore.CitiesDataStore.Current.Cities
                .SelectMany(o => o.PointsOfInterest)
                .Max(p => p.Id);


            var formattedPointOfInterest = new PointsOfInterestDto()
            {
                Id = ++maxId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(formattedPointOfInterest);
            return CreatedAtRoute("GetPointsOfInterest", new { cityId = city.Id, id = formattedPointOfInterest.Id }, formattedPointOfInterest);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInerest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.TryAddModelError("Description", "Name and Description cannot be equal");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            var city = DataStore.CitiesDataStore.Current.Cities
                .FirstOrDefault(p => p.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var point = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (point == null)
            {
                NotFound();
            }

            point.Name = pointOfInterest.Name;
            point.Description = pointOfInterest.Description;
            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatedPointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchPoint)
        {
            if (patchPoint == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                BadRequest();
            }

            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(p => p.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromDataStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterestFromDataStore == null)
            {
                NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestFromDataStore.Name,
                Description = pointOfInterestFromDataStore.Description
            };

            patchPoint.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description)
            {
                ModelState.AddModelError("Description", "Name and Description cannot be equal");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            pointOfInterestFromDataStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromDataStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(p => p.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromDataStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterestFromDataStore == null)
            {
                NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromDataStore);
            mailService.Send("Point of interest deleted.", $"Point of interest {pointOfInterestFromDataStore.Name} with {pointOfInterestFromDataStore.Id} was deleted.");

            return NoContent();
        }
    }
}
