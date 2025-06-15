using mini_hw_2.Interfaces;
using System;
using Domain.Interfaces;

namespace mini_hw_2.Services
{
    public class FeedingOrganizationService : IFeedingOrganizationService
    {
        private readonly IFeedingScheduleRepository _scheduleRepo;

        public FeedingOrganizationService(IFeedingScheduleRepository scheduleRepo)
        {
            _scheduleRepo = scheduleRepo;
        }

        public void FeedAnimal(Guid scheduleId)
        {
            var schedule = _scheduleRepo.GetByIdAsync(scheduleId.ToString()).Result;
            schedule.MarkAsFed();
            Console.WriteLine($"FeedingTimeEvent: {scheduleId}");
        }
    }
}
