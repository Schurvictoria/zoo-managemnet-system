using Domain.Interfaces;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AnimalTransferService
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly IEnclosureRepository _enclosureRepo;

        public AnimalTransferService(IAnimalRepository animalRepo, IEnclosureRepository enclosureRepo)
        {
            _animalRepo = animalRepo;
            _enclosureRepo = enclosureRepo;
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

            Console.WriteLine($"AnimalMovedEvent: {animal.Id}");
        }
    }
} 