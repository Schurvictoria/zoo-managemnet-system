using System;

namespace Application.Interfaces
{
    public interface IFeedingOrganizationService
    {
        void FeedAnimal(Guid feedingScheduleId);
    }
}
