using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class EnclosureRepository : IEnclosureRepository
    {
        private readonly List<Enclosure> _enclosures = new();

        public Task AddAsync(Enclosure enclosure)
        {
            _enclosures.Add(enclosure);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            var enclosure = _enclosures.FirstOrDefault(e => e.Id.ToString() == id);
            if (enclosure != null)
                _enclosures.Remove(enclosure);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Enclosure>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Enclosure>>(_enclosures);
        }

        public Task<Enclosure> GetByIdAsync(string id)
        {
            var enclosure = _enclosures.FirstOrDefault(e => e.Id.ToString() == id);
            return Task.FromResult(enclosure);
        }

        public Task UpdateAsync(Enclosure enclosure)
        {
            var existingEnclosure = _enclosures.FirstOrDefault(e => e.Id == enclosure.Id);
            if (existingEnclosure != null)
            {
                var index = _enclosures.IndexOf(existingEnclosure);
                _enclosures[index] = enclosure;
            }
            return Task.CompletedTask;
        }
    }
}
