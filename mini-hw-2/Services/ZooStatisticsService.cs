using mini_hw_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_hw_2.Services
{
    public class ZooStatisticsService : IZooStatisticsService
    {
        private readonly AnimalRepository _animalRepo;
        private readonly EnclosureRepository _enclosureRepo;

        public ZooStatisticsService(AnimalRepository animalRepo, EnclosureRepository enclosureRepo)
        {
            _animalRepo = animalRepo;
            _enclosureRepo = enclosureRepo;
        }

        public int GetAnimalCount() => _animalRepo.GetAll().Count;

        public int GetFreeEnclosureCount() =>
            _enclosureRepo.GetAll().Count(e => e.Animals.Count < e.Capacity);
    }
}
