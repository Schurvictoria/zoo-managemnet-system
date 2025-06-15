using mini_hw_2.Interfaces;
using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace mini_hw_2.Services
{
    public class AnimalTransferService : IAnimalTransferService
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
            if (!to.AddAnimal(animal))
                throw new InvalidOperationException("Целевой вольер переполнен");

            await _animalRepo.UpdateAsync(animal);
            await _enclosureRepo.UpdateAsync(from);
            await _enclosureRepo.UpdateAsync(to);

            Console.WriteLine($"AnimalMovedEvent: {animal.Id}");
        }

        void IAnimalTransferService.TransferAnimal(Guid animalId, Guid fromEnclosureId, Guid toEnclosureId)
        {
            TransferAnimalAsync(animalId, fromEnclosureId, toEnclosureId).GetAwaiter().GetResult();
        }
    }
}
