using Domain.Interfaces;
using Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ZooStatisticsService
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly IEnclosureRepository _enclosureRepo;

        public ZooStatisticsService(IAnimalRepository animalRepo, IEnclosureRepository enclosureRepo)
        {
            _animalRepo = animalRepo;
            _enclosureRepo = enclosureRepo;
        }

        public async Task<int> GetAnimalCountAsync()
        {
            var animals = await _animalRepo.GetAllAsync();
            return animals.Count();
        }

        public async Task<int> GetFreeEnclosureCountAsync()
        {
            var enclosures = await _enclosureRepo.GetAllAsync();
            return enclosures.Count(e => e.Animals.Count < e.Capacity);
        }
    }
} 