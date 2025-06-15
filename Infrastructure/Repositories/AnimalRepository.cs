using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly List<Animal> _animals = new();

        public Task AddAsync(Animal animal)
        {
            _animals.Add(animal);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            var animal = _animals.FirstOrDefault(x => x.Id.ToString() == id);
            if (animal != null)
                _animals.Remove(animal);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Animal>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Animal>>(_animals);
        }

        public Task<Animal> GetByIdAsync(string id)
        {
            var animal = _animals.FirstOrDefault(x => x.Id.ToString() == id);
            return Task.FromResult(animal);
        }

        public Task<IEnumerable<Animal>> GetAnimalsByEnclosureAsync(string enclosureId)
        {
            var animals = _animals.Where(x => x.Enclosure.Id.ToString() == enclosureId);
            return Task.FromResult(animals);
        }

        public Task UpdateAsync(Animal animal)
        {
            var existingAnimal = _animals.FirstOrDefault(x => x.Id == animal.Id);
            if (existingAnimal != null)
            {
                var index = _animals.IndexOf(existingAnimal);
                _animals[index] = animal;
            }
            return Task.CompletedTask;
        }
    }
}
