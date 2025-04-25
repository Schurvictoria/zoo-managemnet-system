using mini_hw_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_hw_2.Services
{
    public class FeedingOrganizationService : IFeedingOrganizationService
    {
        private readonly FeedingScheduleRepository _scheduleRepo;

        public FeedingOrganizationService(FeedingScheduleRepository scheduleRepo)
        {
            _scheduleRepo = scheduleRepo;
        }

        public void FeedAnimal(Guid scheduleId)
        {
            var schedule = _scheduleRepo.GetById(scheduleId);
            schedule.MarkAsFed();
            Console.WriteLine($"FeedingTimeEvent: {scheduleId}");
        }
    }
}
