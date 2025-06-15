using Domain.Entities;

namespace Domain.Interfaces;

public interface IFeedingScheduleRepository
{
    Task<FeedingSchedule> GetByIdAsync(string id);
    Task<IEnumerable<FeedingSchedule>> GetAllAsync();
    Task AddAsync(FeedingSchedule feedingSchedule);
    Task UpdateAsync(FeedingSchedule feedingSchedule);
    Task DeleteAsync(string id);
}
