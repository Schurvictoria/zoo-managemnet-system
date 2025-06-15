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

        public Task UpdateAsync(Animal animal)
        {
            // Для in-memory коллекции ничего делать не нужно
            return Task.CompletedTask;
        }
    }
}
