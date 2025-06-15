using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class FeedingScheduleRepository : IFeedingScheduleRepository
    {
        private readonly List<FeedingSchedule> _schedules = new();

        public Task AddAsync(FeedingSchedule feedingSchedule)
        {
            _schedules.Add(feedingSchedule);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            var schedule = _schedules.FirstOrDefault(s => s.Id.ToString() == id);
            if (schedule != null)
                _schedules.Remove(schedule);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<FeedingSchedule>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<FeedingSchedule>>(_schedules);
        }

        public Task<FeedingSchedule> GetByIdAsync(string id)
        {
            var schedule = _schedules.FirstOrDefault(s => s.Id.ToString() == id);
            return Task.FromResult(schedule);
        }

        public Task UpdateAsync(FeedingSchedule feedingSchedule)
        {
            return Task.CompletedTask;
        }
    }
}
