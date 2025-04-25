using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AnimalRepository
    {
        private readonly List<Animal> _animals = new();

        public void Add(Animal animal) => _animals.Add(animal);
        public void Remove(Animal animal) => _animals.Remove(animal);
        public Animal GetById(Guid id) => _animals.First(x => x.Id == id);
        public List<Animal> GetAll() => _animals;
    }
}
