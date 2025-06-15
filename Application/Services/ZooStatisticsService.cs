using Domain.Interfaces;
using Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using System;

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

        public async Task TransferAnimalAsync(Guid animalId, Guid fromId, Guid toId)
        {
            var animal = await _animalRepo.GetByIdAsync(animalId.ToString());
            var from = await _enclosureRepo.GetByIdAsync(fromId.ToString());
            var to = await _enclosureRepo.GetByIdAsync(toId.ToString());

            if (animal == null || from == null || to == null)
                throw new InvalidOperationException("Animal or enclosure not found");

            if (animal.Enclosure == null || animal.Enclosure.Id != from.Id)
                throw new InvalidOperationException("Animal is not in the specified source enclosure");

            from.RemoveAnimal(animal);
            if (!to.AddAnimal(animal))
                throw new InvalidOperationException("Target enclosure is full");

            animal.UpdateEnclosure(to);

            await _animalRepo.UpdateAsync(animal);
            await _enclosureRepo.UpdateAsync(from);
            await _enclosureRepo.UpdateAsync(to);
        }
    }
} 