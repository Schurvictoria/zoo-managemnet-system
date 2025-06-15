using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class TransferAnimalRequest
    {
        public string AnimalId { get; set; }
        public string FromEnclosureId { get; set; }
        public string ToEnclosureId { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;
        public TransferController(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository)
        {
            _animalRepository = animalRepository;
            _enclosureRepository = enclosureRepository;
        }

        [HttpPost]
        public async Task<IActionResult> TransferAnimal([FromBody] TransferAnimalRequest request)
        {
            var animal = await _animalRepository.GetByIdAsync(request.AnimalId);
            if (animal == null)
                return NotFound(new { error = "Animal not found", animalId = request.AnimalId });
            var from = await _enclosureRepository.GetByIdAsync(request.FromEnclosureId);
            if (from == null)
                return NotFound(new { error = "Source enclosure not found", enclosureId = request.FromEnclosureId });
            var to = await _enclosureRepository.GetByIdAsync(request.ToEnclosureId);
            if (to == null)
                return NotFound(new { error = "Target enclosure not found", enclosureId = request.ToEnclosureId });
            // TODO: Переместить животное между вольерами
            return Ok();
        }
    }
}
