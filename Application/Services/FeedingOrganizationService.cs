using Domain.Interfaces;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FeedingOrganizationService
    {
        private readonly IFeedingScheduleRepository _scheduleRepo;

        public FeedingOrganizationService(IFeedingScheduleRepository scheduleRepo)
        {
            _scheduleRepo = scheduleRepo;
        }

        public async Task FeedAnimalAsync(Guid scheduleId)
        {
            var schedule = await _scheduleRepo.GetByIdAsync(scheduleId.ToString());
            schedule.MarkAsCompleted();
            Console.WriteLine($"FeedingTimeEvent: {scheduleId}");
        }
    }
} 