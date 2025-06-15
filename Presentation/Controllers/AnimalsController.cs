using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Presentation.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Presentation.Controllers
{
    public class AddAnimalRequest
    {
        public required string Name { get; set; }
        public required string Species { get; set; }
        public required string FoodType { get; set; }
        public required string EnclosureId { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Gender { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly AnimalService _animalService;
        private readonly IEnclosureRepository _enclosureRepository;
        private readonly ILogger<AnimalsController> _logger;

        public AnimalsController(
            AnimalService animalService, 
            IEnclosureRepository enclosureRepository,
            ILogger<AnimalsController> logger)
        {
            _animalService = animalService;
            _enclosureRepository = enclosureRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all animals with optional filtering and pagination
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAnimals(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? species = null,
            [FromQuery] string? enclosureId = null)
        {
            try
            {
                var animals = await _animalService.GetAllAnimalsAsync();
                if (!string.IsNullOrEmpty(species))
                    animals = animals.Where(a => a.Species == species);
                if (!string.IsNullOrEmpty(enclosureId))
                    animals = animals.Where(a => a.Enclosure != null && a.Enclosure.Id.ToString() == enclosureId);

                var animalList = animals
                    .Select(a => new AnimalResponse
                    {
                        Id = a.Id.ToString(),
                        Name = a.Name,
                        Species = a.Species,
                        FoodType = a.FoodType,
                        EnclosureId = a.Enclosure.Id.ToString(),
                        BirthDate = a.BirthDate,
                        Gender = a.Gender.ToString()
                    });

                return Ok(animalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting animals");
                return StatusCode(500, "An error occurred while retrieving animals");
            }
        }

        /// <summary>
        /// Adds a new animal to the zoo
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAnimal([FromBody] AddAnimalRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var enclosure = await _enclosureRepository.GetByIdAsync(request.EnclosureId);
                if (enclosure == null)
                {
                    return NotFound(new { error = "Enclosure not found", enclosureId = request.EnclosureId });
                }

                if (enclosure.Capacity <= 0)
                {
                    return BadRequest(new { error = "Enclosure capacity must be greater than 0" });
                }

                var currentAnimals = await _animalService.GetAllAnimalsAsync();
                var enclosureAnimals = currentAnimals.Count(a => a.Enclosure != null && a.Enclosure.Id == enclosure.Id);
                if (enclosureAnimals >= enclosure.Capacity)
                {
                    return BadRequest(new { error = "Enclosure is at full capacity" });
                }

                if (!Enum.TryParse<Gender>(request.Gender, out var gender))
                {
                    var validGenders = Enum.GetNames(typeof(Gender));
                    return BadRequest(new { error = "Invalid gender value", validValues = validGenders });
                }

                var animalId = await _animalService.AddAnimalAsync(
                    request.Species,
                    request.Name,
                    request.BirthDate,
                    gender,
                    request.FoodType,
                    enclosure);

                _logger.LogInformation("Added new animal with name: {AnimalName}", request.Name);
                return Ok(new { id = animalId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding animal");
                return StatusCode(500, "An error occurred while adding the animal");
            }
        }

        /// <summary>
        /// Deletes an animal from the zoo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(string id)
        {
            try
            {
                var animal = await _animalService.GetAllAnimalsAsync()
                    .ContinueWith(t => t.Result.FirstOrDefault(a => a.Id.ToString() == id));
                    
                if (animal == null)
                {
                    return NotFound(new { error = "Animal not found", animalId = id });
                }

                await _animalService.DeleteAnimalAsync(Guid.Parse(id));
                _logger.LogInformation("Deleted animal: {AnimalId}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting animal {AnimalId}", id);
                return StatusCode(500, "An error occurred while deleting the animal");
            }
        }
    }
}
