using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AddFeedingRequest
    {
        public string AnimalId { get; set; }
        public string FoodType { get; set; }
        public string Time { get; set; } // Можно заменить на DateTime, если нужно
    }

    [ApiController]
    [Route("api/[controller]")]
    public class FeedingScheduleController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;
        public FeedingScheduleController(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        [HttpGet]
        public IActionResult GetSchedule()
        {
            // TODO: Получить расписание кормлений
            return Ok(new[] { "Кормление 1", "Кормление 2" });
        }

        [HttpPost]
        public async Task<IActionResult> AddFeeding([FromBody] AddFeedingRequest request)
        {
            var animal = await _animalRepository.GetByIdAsync(request.AnimalId);
            if (animal == null)
                return NotFound(new { error = "Animal not found", animalId = request.AnimalId });
            // TODO: Добавить кормление
            return Ok();
        }
    }
}
