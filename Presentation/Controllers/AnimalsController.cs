using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;

namespace Presentation.Controllers
{
    public class AddAnimalRequest
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string FoodType { get; set; }
        public string EnclosureId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly AnimalService _animalService;
        private readonly IEnclosureRepository _enclosureRepository;
        public AnimalsController(AnimalService animalService, IEnclosureRepository enclosureRepository)
        {
            _animalService = animalService;
            _enclosureRepository = enclosureRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnimals()
        {
            var animals = await _animalService.GetAllAnimalsAsync();
            return Ok(animals);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimal([FromBody] AddAnimalRequest request)
        {
            var enclosure = await _enclosureRepository.GetByIdAsync(request.EnclosureId);
            if (enclosure == null)
            {
                return NotFound(new { error = "Enclosure not found", enclosureId = request.EnclosureId });
            }
            var gender = Enum.TryParse<Gender>(request.Gender, out var g) ? g : Gender.Male;
            await _animalService.AddAnimalAsync(request.Species, request.Name, request.BirthDate, gender, request.FoodType);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(string id)
        {
            await _animalService.DeleteAnimalAsync(id);
            return Ok();
        }
    }
}
