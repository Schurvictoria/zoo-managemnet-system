using mini_hw_2.Interfaces;
using System;
using System.Linq;
using Domain.Interfaces;

namespace mini_hw_2.Services
{
    public class ZooStatisticsService : IZooStatisticsService
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly IEnclosureRepository _enclosureRepo;

        public ZooStatisticsService(IAnimalRepository animalRepo, IEnclosureRepository enclosureRepo)
        {
            _animalRepo = animalRepo;
            _enclosureRepo = enclosureRepo;
        }

        public int GetAnimalCount()
        {
            return _animalRepo.GetAllAsync().Result.Count();
        }

        public int GetFreeEnclosureCount()
        {
            return _enclosureRepo.GetAllAsync().Result.Count(e => e.Animals.Count < e.Capacity);
        }
    }
}
