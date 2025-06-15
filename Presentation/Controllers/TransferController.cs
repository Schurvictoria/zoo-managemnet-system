using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Application.Services;

namespace Presentation.Controllers
{
    public class TransferAnimalRequest
    {
        public required string AnimalId { get; set; }
        public required string FromEnclosureId { get; set; }
        public required string ToEnclosureId { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;
        private readonly AnimalTransferService _transferService;
        private readonly ILogger<TransferController> _logger;

        public TransferController(
            IAnimalRepository animalRepository, 
            IEnclosureRepository enclosureRepository,
            AnimalTransferService transferService,
            ILogger<TransferController> logger)
        {
            _animalRepository = animalRepository;
            _enclosureRepository = enclosureRepository;
            _transferService = transferService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> TransferAnimal([FromBody] TransferAnimalRequest request)
        {
            try
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

                await _transferService.TransferAnimalAsync(
                    Guid.Parse(request.AnimalId),
                    Guid.Parse(request.FromEnclosureId),
                    Guid.Parse(request.ToEnclosureId));

                _logger.LogInformation("Transferred animal {AnimalId} from enclosure {FromId} to {ToId}", 
                    request.AnimalId, request.FromEnclosureId, request.ToEnclosureId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error transferring animal {AnimalId}", request.AnimalId);
                return StatusCode(500, "An error occurred while transferring the animal");
            }
        }
    }
}
