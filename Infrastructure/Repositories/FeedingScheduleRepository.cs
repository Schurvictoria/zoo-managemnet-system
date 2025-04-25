using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooManagementSystem.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class FeedingScheduleRepository
    {
        private readonly List<FeedingSchedule> _schedules = new();

        public void Add(FeedingSchedule fs) => _schedules.Add(fs);
        public FeedingSchedule GetById(Guid id) => _schedules.First(s => s.Id == id);
        public List<FeedingSchedule> GetAll() => _schedules;
    }
}
