using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_hw_2.Interfaces
{
    public interface IFeedingOrganizationService
    {
        void FeedAnimal(Guid feedingScheduleId);
    }
}
