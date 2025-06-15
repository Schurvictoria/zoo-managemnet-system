using System;
using Domain.Interfaces;
using Application.Interfaces;

namespace Application.Services
{
    public class FeedingOrganizationService : IFeedingOrganizationService
    {
        private readonly IFeedingScheduleRepository _feedingScheduleRepo;

        public FeedingOrganizationService(IFeedingScheduleRepository feedingScheduleRepo)
        {
            _feedingScheduleRepo = feedingScheduleRepo;
        }

        public void FeedAnimal(Guid feedingScheduleId)
        {
            var schedule = _feedingScheduleRepo.GetByIdAsync(feedingScheduleId.ToString()).Result;
            if (schedule != null)
            {
                // Реализация кормления животного
                Console.WriteLine($"Кормление животного по расписанию {feedingScheduleId}");
            }
        }
    }
} 