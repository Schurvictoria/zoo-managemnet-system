using Microsoft.AspNetCore.Mvc;
using Application.Services;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ZooStatisticsService _zooStatisticsService;
        public StatisticsController(ZooStatisticsService zooStatisticsService)
        {
            _zooStatisticsService = zooStatisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var animalCount = await _zooStatisticsService.GetAnimalCountAsync();
            var freeEnclosureCount = await _zooStatisticsService.GetFreeEnclosureCountAsync();
            return Ok(new { AnimalsCount = animalCount, FreeEnclosures = freeEnclosureCount });
        }
    }
}
