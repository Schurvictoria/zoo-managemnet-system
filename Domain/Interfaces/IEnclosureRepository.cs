using Domain.Entities;

namespace Domain.Interfaces;

public interface IEnclosureRepository
{
    Task<Enclosure> GetByIdAsync(string id);
    Task<IEnumerable<Enclosure>> GetAllAsync();
    Task AddAsync(Enclosure enclosure);
    Task UpdateAsync(Enclosure enclosure);
    Task DeleteAsync(string id);
}