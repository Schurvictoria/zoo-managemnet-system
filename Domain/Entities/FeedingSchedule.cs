using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FeedingSchedule
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Animal Animal { get; private set; }
        public DateTime FeedingTime { get; private set; }
        public string FoodType { get; private set; }
        public bool IsCompleted { get; private set; }

        public FeedingSchedule(Animal animal, DateTime feedingTime, string foodType)
        {
            Animal = animal;
            FeedingTime = feedingTime;
            FoodType = foodType;
        }

        public void Reschedule(DateTime newTime) => FeedingTime = newTime;

        public void MarkAsFed()
        {
            Animal.Feed();
            Console.WriteLine($"Кормление {Animal.Name} завершено.");
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }
    }
}
