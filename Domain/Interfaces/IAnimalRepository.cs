using Domain.Entities;

namespace Domain.Interfaces;

public interface IAnimalRepository
{
    Task<Animal> GetByIdAsync(string id);
    Task<IEnumerable<Animal>> GetAllAsync();
    Task AddAsync(Animal animal);
    Task UpdateAsync(Animal animal);
    Task DeleteAsync(string id);
    Task<IEnumerable<Animal>> GetAnimalsByEnclosureAsync(string enclosureId);
}