using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EnclosureRepository
    {
        private readonly List<Enclosure> _enclosures = new();

        public void Add(Enclosure e) => _enclosures.Add(e);
        public Enclosure GetById(Guid id) => _enclosures.First(e => e.Id == id);
        public List<Enclosure> GetAll() => _enclosures;
    }
}
