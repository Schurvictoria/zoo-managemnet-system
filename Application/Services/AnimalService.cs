using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class AnimalService
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            return await _animalRepository.GetAllAsync();
        }

        public async Task<Guid> AddAnimalAsync(string species, string name, DateTime birthDate, Gender gender, string favoriteFood, Enclosure enclosure)
        {
            var animal = new Animal(species, name, birthDate, gender, favoriteFood, enclosure);
            await _animalRepository.AddAsync(animal);
            return animal.Id;
        }

        public async Task DeleteAnimalAsync(Guid id)
        {
            await _animalRepository.DeleteAsync(id.ToString());
        }
    }
}
