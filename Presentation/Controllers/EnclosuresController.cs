using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AddEnclosureRequest
    {
        public string Type { get; set; }
        public int Capacity { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class EnclosuresController : ControllerBase
    {
        private readonly IEnclosureRepository _enclosureRepository;

        public EnclosuresController(IEnclosureRepository enclosureRepository)
        {
            _enclosureRepository = enclosureRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEnclosures()
        {
            var enclosures = await _enclosureRepository.GetAllAsync();
            return Ok(enclosures);
        }

        [HttpPost]
        public async Task<IActionResult> AddEnclosure([FromBody] AddEnclosureRequest request)
        {
            var validTypes = Enum.GetNames(typeof(EnclosureType));
            if (!Enum.TryParse<EnclosureType>(request.Type, true, out var type))
            {
                return BadRequest(new {
                    error = "Invalid enclosure type",
                    validTypes
                });
            }

            var enclosure = new Enclosure(type, request.Capacity);
            await _enclosureRepository.AddAsync(enclosure);
            return Ok(enclosure);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnclosure(string id)
        {
            var enclosure = await _enclosureRepository.GetByIdAsync(id);
            if (enclosure == null)
            {
                return NotFound();
            }

            await _enclosureRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
