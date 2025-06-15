using Xunit;
using Domain.Entities;
using System;

namespace Tests
{
    public class DomainEntitiesTests
    {
        [Fact]
        public void Animal_Feed_Heal_SetSick_UpdateEnclosure_Works()
        {
            var enclosure1 = new Enclosure(EnclosureType.Predator, 2);
            var enclosure2 = new Enclosure(EnclosureType.Herbivore, 2);
            var animal = new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", enclosure1);

            // Feed (просто вызов, проверяем что не падает)
            animal.Feed();

            // SetSick и Heal
            animal.SetSick();
            Assert.Equal(AnimalStatus.Sick, animal.Status);
            animal.Heal();
            Assert.Equal(AnimalStatus.Healthy, animal.Status);

            // UpdateEnclosure
            animal.UpdateEnclosure(enclosure2);
            Assert.Equal(enclosure2, animal.Enclosure);
        }

        [Fact]
        public void Enclosure_AddAnimal_RemoveAnimal_Capacity()
        {
            var enclosure = new Enclosure(EnclosureType.Predator, 1);
            var animal1 = new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", enclosure);
            var animal2 = new Animal("Tiger", "Shere Khan", DateTime.Now, Gender.Male, "Meat", enclosure);

            // Добавить животное (есть место)
            var added = enclosure.AddAnimal(animal1);
            Assert.True(added);
            Assert.Contains(animal1, enclosure.Animals);

            // Добавить второе (нет места)
            var added2 = enclosure.AddAnimal(animal2);
            Assert.False(added2);

            // Удалить животное
            enclosure.RemoveAnimal(animal1);
            Assert.DoesNotContain(animal1, enclosure.Animals);
        }

        [Fact]
        public void Enclosure_Clean_DoesNotThrow()
        {
            var enclosure = new Enclosure(EnclosureType.Predator, 1);
            enclosure.Clean(); // просто вызов, проверяем что не падает
        }

        [Fact]
        public void FeedingSchedule_Reschedule_UpdateAnimal_UpdateFoodType_UpdateFeedingTime_MarkAsFed_MarkAsCompleted()
        {
            var enclosure = new Enclosure(EnclosureType.Herbivore, 2);
            var animal = new Animal("Elephant", "Dumbo", DateTime.Now, Gender.Male, "Fruit", enclosure);
            var schedule = new FeedingSchedule(animal, DateTime.Now, "Fruit");

            // Reschedule
            var newTime = DateTime.Now.AddHours(1);
            schedule.Reschedule(newTime);
            Assert.Equal(newTime, schedule.FeedingTime);

            // UpdateAnimal
            var animal2 = new Animal("Giraffe", "Melman", DateTime.Now, Gender.Male, "Leaves", enclosure);
            schedule.UpdateAnimal(animal2);
            Assert.Equal(animal2, schedule.Animal);

            // UpdateFoodType
            schedule.UpdateFoodType("Leaves");
            Assert.Equal("Leaves", schedule.FoodType);

            // UpdateFeedingTime
            var anotherTime = DateTime.Now.AddHours(2);
            schedule.UpdateFeedingTime(anotherTime);
            Assert.Equal(anotherTime, schedule.FeedingTime);

            // MarkAsFed (просто вызов, проверяем что не падает)
            schedule.MarkAsFed();

            // MarkAsCompleted
            schedule.MarkAsCompleted();
            Assert.True(schedule.IsCompleted);
        }
    }
}
