using mini_hw_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mini_hw_2.Domain.Entities;


namespace mini_hw_2.Services
{
    public class AnimalTransferService : IAnimalTransferService
    {
        private readonly AnimalRepository _animalRepo;
        private readonly EnclosureRepository _enclosureRepo;

        public AnimalTransferService(AnimalRepository animalRepo, EnclosureRepository enclosureRepo)
        {
            _animalRepo = animalRepo;
            _enclosureRepo = enclosureRepo;
        }

        public void TransferAnimal(Guid animalId, Guid fromId, Guid toId)
        {
            var animal = _animalRepo.GetById(animalId);
            var from = _enclosureRepo.GetById(fromId);
            var to = _enclosureRepo.GetById(toId);

            from.RemoveAnimal(animal);
            if (!to.AddAnimal(animal))
                throw new InvalidOperationException("Целевой вольер переполнен");

            Console.WriteLine($"AnimalMovedEvent: {animal.Id}");
        }
    }
}
