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

        public async Task AddAnimalAsync(string species, string name, DateTime birthDate, Gender gender, string favoriteFood)
        {
            var animal = new Animal(species, name, birthDate, gender, favoriteFood);
            await _animalRepository.AddAsync(animal);
        }

        public async Task DeleteAnimalAsync(string id)
        {
            await _animalRepository.DeleteAsync(id);
        }
    }
}
