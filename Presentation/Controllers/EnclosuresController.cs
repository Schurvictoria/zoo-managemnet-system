using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Domain.Entities;
using Presentation.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Presentation.Controllers
{
    public class AddEnclosureRequest
    {
        public required string Type { get; set; }
        public int Capacity { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class EnclosuresController : ControllerBase
    {
        private readonly IEnclosureRepository _enclosureRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly ILogger<EnclosuresController> _logger;

        public EnclosuresController(
            IEnclosureRepository enclosureRepository,
            IAnimalRepository animalRepository,
            ILogger<EnclosuresController> logger)
        {
            _enclosureRepository = enclosureRepository;
            _animalRepository = animalRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all enclosures with optional filtering and pagination
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEnclosures(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? type = null)
        {
            try
            {
                var enclosures = await _enclosureRepository.GetAllAsync();

                // Apply filters
                if (!string.IsNullOrEmpty(type))
                {
                    if (!Enum.TryParse<EnclosureType>(type, out var enclosureType))
                    {
                        return BadRequest(new { error = "Invalid enclosure type" });
                    }
                    enclosures = enclosures.Where(e => e.Type == enclosureType);
                }

                // Apply pagination
                var totalCount = enclosures.Count();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                var pagedEnclosures = enclosures
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new EnclosureResponse
                    {
                        Id = e.Id.ToString(),
                        Type = e.Type.ToString(),
                        Capacity = e.Capacity,
                        CurrentOccupancy = e.Animals.Count
                    });

                return Ok(new
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize,
                    Enclosures = pagedEnclosures
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting enclosures");
                return StatusCode(500, "An error occurred while retrieving enclosures");
            }
        }

        /// <summary>
        /// Adds a new enclosure to the zoo
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddEnclosure([FromBody] AddEnclosureRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!Enum.TryParse<EnclosureType>(request.Type, out var enclosureType))
                {
                    return BadRequest(new { error = "Invalid enclosure type" });
                }

                var enclosure = new Enclosure(enclosureType, request.Capacity);
                await _enclosureRepository.AddAsync(enclosure);
                
                _logger.LogInformation("Added new enclosure: {EnclosureId}", enclosure.Id);
                return Ok(new { id = enclosure.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding enclosure");
                return StatusCode(500, "An error occurred while adding the enclosure");
            }
        }

        /// <summary>
        /// Deletes an enclosure from the zoo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnclosure(string id)
        {
            try
            {
                var enclosure = await _enclosureRepository.GetByIdAsync(id);
                if (enclosure == null)
                {
                    return NotFound(new { error = "Enclosure not found", enclosureId = id });
                }

                // Check if enclosure has animals
                var animals = await _animalRepository.GetAnimalsByEnclosureAsync(id);
                if (animals.Any())
                {
                    return BadRequest(new { error = "Cannot delete enclosure with animals" });
                }

                await _enclosureRepository.DeleteAsync(id);
                _logger.LogInformation("Deleted enclosure: {EnclosureId}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting enclosure {EnclosureId}", id);
                return StatusCode(500, "An error occurred while deleting the enclosure");
            }
        }

        [HttpGet("{id}/animals")]
        public async Task<IActionResult> GetEnclosureAnimals(string id)
        {
            try
            {
                var animals = await _animalRepository.GetAnimalsByEnclosureAsync(id);
                return Ok(animals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting animals for enclosure {EnclosureId}", id);
                return StatusCode(500, "An error occurred while retrieving animals for the enclosure");
            }
        }
    }
}
