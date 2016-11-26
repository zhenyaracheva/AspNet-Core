namespace CityInfo.Core.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Microsoft.AspNetCore.JsonPatch;
    using DataStore;
    using Microsoft.Extensions.Logging;
    using Services.RepositoryServices;
    using System.Collections.Generic;
    using AutoMapper;
    using Entities;

    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> logger;
        private IMailService mailService;
        private ICityInfoRepository cityInfo;

        public PointsOfInterestController(ILogger<PointsOfInterestController> loggerService, IMailService mail, ICityInfoRepository cityRepo)
        {
            logger = loggerService;
            mailService = mail;
            cityInfo = cityRepo;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = cityInfo.GetCity(cityId, true);

            if (city == null)
            {
                this.logger.LogInformation($"No city with {cityId} id!");
                return NotFound();
            }

            var result = Mapper.Map<IEnumerable<PointsOfInterestDto>>(city.PointsOfInterest);

            return Ok(result);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointsOfInterest")]
        public IActionResult GetPointsOfInterest(int cityId, int id)
        {
            var city = cityInfo.GetCity(cityId, true);

            if (city == null)
            {
                this.logger.LogInformation($"No city with {cityId} id!");
                return NotFound();
            }

            var point = cityInfo.GetPointOfInterestFromCity(cityId, id);

            if (point == null)
            {
                this.logger.LogInformation($"No city with {cityId} id!");
                return NotFound();
            }

            var result = Mapper.Map<PointsOfInterestDto>(point);

            return Ok(result);
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

            var city = cityInfo.GetCity(cityId, true);

            if (city == null)
            {
                return NotFound();
            }

            var formattedPointOfInterest = Mapper.Map<PointOfInterest>(pointOfInterest);
            var isSaved = cityInfo.AddPointOfInterestForCity(cityId, formattedPointOfInterest);

            if (!isSaved)
            {
                return StatusCode(500, "Problem happend while handling your request!");
            }

            var createdPoint = Mapper.Map<Models.PointsOfInterestDto>(formattedPointOfInterest);
            return CreatedAtRoute("GetPointsOfInterest", new { cityId = city.Id, id = createdPoint.Id }, createdPoint);
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

            var city = cityInfo.GetCity(cityId, true);

            if (city == null)
            {
                return NotFound();
            }

            var point = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (point == null)
            {
                NotFound();
            }

            Mapper.Map(pointOfInterest, point);

            if (!cityInfo.Save())
            {
                return StatusCode(500, "Something happened");
            }
            
            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatedPointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchPoint)
        {
            if (patchPoint == null)
            {
                return BadRequest();
            }
            
            var city = cityInfo.GetCity(cityId, true);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromDataStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterestFromDataStore == null)
            {
                NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestFromDataStore);
           
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
            
            Mapper.Map(pointOfInterestToPatch, pointOfInterestFromDataStore);

            if(!cityInfo.Save())
            {
                return StatusCode(500, "Error handling Update request");
            }

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = cityInfo.GetCity(cityId, true);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromDataStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterestFromDataStore == null)
            {
                NotFound();
            }

            var IsSaved = cityInfo.DeletePointofInterest(pointOfInterestFromDataStore);

            if(!IsSaved)
            {
                return StatusCode(500, "Something happened");
            }

            city.PointsOfInterest.Remove(pointOfInterestFromDataStore);
            mailService.Send("Point of interest deleted.", $"Point of interest {pointOfInterestFromDataStore.Name} with {pointOfInterestFromDataStore.Id} was deleted.");

            return NoContent();
        }
    }
}
