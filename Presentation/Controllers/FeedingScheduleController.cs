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
    public class AddFeedingRequest
    {
        public required string AnimalId { get; set; }
        public required string FoodType { get; set; }
        public DateTime FeedingTime { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class FeedingScheduleController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IFeedingScheduleRepository _feedingScheduleRepository;
        private readonly ILogger<FeedingScheduleController> _logger;

        public FeedingScheduleController(
            IAnimalRepository animalRepository,
            IFeedingScheduleRepository feedingScheduleRepository,
            ILogger<FeedingScheduleController> logger)
        {
            _animalRepository = animalRepository;
            _feedingScheduleRepository = feedingScheduleRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all feeding schedules with optional filtering and pagination
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSchedule(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? animalId = null,
            [FromQuery] DateTime? date = null)
        {
            try
            {
                var schedules = await _feedingScheduleRepository.GetAllAsync();

                // Apply filters
                if (!string.IsNullOrEmpty(animalId))
                    schedules = schedules.Where(f => f.Animal.Id.ToString() == animalId);
                if (date.HasValue)
                    schedules = schedules.Where(f => f.FeedingTime.Date == date.Value.Date);

                // Apply pagination
                var totalCount = schedules.Count();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                var pagedSchedules = schedules
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                return Ok(new
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize,
                    Schedules = pagedSchedules
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting feeding schedules");
                return StatusCode(500, "An error occurred while retrieving feeding schedules");
            }
        }

        /// <summary>
        /// Adds a new feeding schedule
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddFeeding([FromBody] AddFeedingRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var animal = await _animalRepository.GetByIdAsync(request.AnimalId);
                if (animal == null)
                {
                    return NotFound(new { error = "Animal not found", animalId = request.AnimalId });
                }

                var schedule = new FeedingSchedule(animal, request.FeedingTime, request.FoodType);
                await _feedingScheduleRepository.AddAsync(schedule);

                _logger.LogInformation("Added new feeding schedule for animal: {AnimalId}", animal.Id);
                return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding feeding schedule");
                return StatusCode(500, "An error occurred while adding the feeding schedule");
            }
        }

        /// <summary>
        /// Updates a feeding schedule
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeeding(Guid id, [FromBody] AddFeedingRequest request)
        {
            try
            {
                var schedule = await _feedingScheduleRepository.GetByIdAsync(id.ToString());
                if (schedule == null)
                {
                    return NotFound(new { error = "Feeding schedule not found", id = id });
                }

                var animal = await _animalRepository.GetByIdAsync(request.AnimalId);
                if (animal == null)
                {
                    return NotFound(new { error = "Animal not found", animalId = request.AnimalId });
                }

                schedule.UpdateAnimal(animal);
                schedule.UpdateFeedingTime(request.FeedingTime);
                schedule.UpdateFoodType(request.FoodType);

                await _feedingScheduleRepository.UpdateAsync(schedule);
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating feeding schedule {Id}", id);
                return StatusCode(500, "An error occurred while updating the feeding schedule");
            }
        }

        /// <summary>
        /// Deletes a feeding schedule
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeeding(Guid id)
        {
            try
            {
                var schedule = await _feedingScheduleRepository.GetByIdAsync(id.ToString());
                if (schedule == null)
                {
                    return NotFound(new { error = "Feeding schedule not found", id = id });
                }

                await _feedingScheduleRepository.DeleteAsync(id.ToString());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting feeding schedule {Id}", id);
                return StatusCode(500, "An error occurred while deleting the feeding schedule");
            }
        }
    }
}
