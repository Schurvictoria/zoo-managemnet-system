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

            from.RemoveAnimal(animal);
            to.AddAnimal(animal);

            // Обновляем состояние в репозиториях
            await _enclosureRepo.UpdateAsync(from);
            await _enclosureRepo.UpdateAsync(to);

            Console.WriteLine($"AnimalMovedEvent: {animal.Id}");
        }
    }
} 