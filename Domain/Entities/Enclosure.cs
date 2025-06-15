using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public enum EnclosureType { Predator, Herbivore, Bird, Aquarium }

    public class Enclosure
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public EnclosureType Type { get; private set; }
        public int Capacity { get; private set; }
        private readonly List<Animal> _animals = new();
        public IReadOnlyCollection<Animal> Animals => _animals.AsReadOnly();

        public Enclosure(EnclosureType type, int capacity)
        {
            Type = type;
            Capacity = capacity;
        }

        public bool AddAnimal(Animal animal)
        {
            if (_animals.Count >= Capacity) return false;
            _animals.Add(animal);
            return true;
        }

        public void RemoveAnimal(Animal animal) => _animals.Remove(animal);
        
        // This method should be implemented with proper logging or event system
        public void Clean() { }
    }
}
