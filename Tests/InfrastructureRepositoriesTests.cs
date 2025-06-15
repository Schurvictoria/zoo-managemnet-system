using Xunit;
using Infrastructure.Repositories;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public class InfrastructureRepositoriesTests
    {
        [Fact]
        public async Task AnimalRepository_CRUD_Works()
        {
            var repo = new AnimalRepository();
            var enclosure = new Enclosure(EnclosureType.Predator, 2);
            var animal = new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", enclosure);

            // Add
            await repo.AddAsync(animal);
            var all = await repo.GetAllAsync();
            Assert.Contains(all, a => a.Id == animal.Id);

            // GetById
            var byId = await repo.GetByIdAsync(animal.Id.ToString());
            Assert.Equal(animal.Id, byId.Id);

            // Update
            var updatedAnimal = new Animal("Lion", "Simba", animal.BirthDate, Gender.Male, "Meat", enclosure);
            typeof(Animal).GetProperty("Id").SetValue(updatedAnimal, animal.Id); // set same Id
            await repo.UpdateAsync(updatedAnimal);
            var byIdAfterUpdate = await repo.GetByIdAsync(animal.Id.ToString());
            Assert.Equal(updatedAnimal.Name, byIdAfterUpdate.Name);

            // GetAnimalsByEnclosure
            var animalsInEnclosure = await repo.GetAnimalsByEnclosureAsync(enclosure.Id.ToString());
            Assert.Contains(animalsInEnclosure, a => a.Id == animal.Id);

            // Delete
            await repo.DeleteAsync(animal.Id.ToString());
            var allAfterDelete = await repo.GetAllAsync();
            Assert.DoesNotContain(allAfterDelete, a => a.Id == animal.Id);
        }

        [Fact]
        public async Task EnclosureRepository_CRUD_Works()
        {
            var repo = new EnclosureRepository();
            var enclosure = new Enclosure(EnclosureType.Predator, 2);

            // Add
            await repo.AddAsync(enclosure);
            var all = await repo.GetAllAsync();
            Assert.Contains(all, e => e.Id == enclosure.Id);

            // GetById
            var byId = await repo.GetByIdAsync(enclosure.Id.ToString());
            Assert.Equal(enclosure.Id, byId.Id);

            // Update
            var updatedEnclosure = new Enclosure(EnclosureType.Predator, 3);
            typeof(Enclosure).GetProperty("Id").SetValue(updatedEnclosure, enclosure.Id); // set same Id
            await repo.UpdateAsync(updatedEnclosure);
            var byIdAfterUpdate = await repo.GetByIdAsync(enclosure.Id.ToString());
            Assert.Equal(3, byIdAfterUpdate.Capacity);

            // Delete
            await repo.DeleteAsync(enclosure.Id.ToString());
            var allAfterDelete = await repo.GetAllAsync();
            Assert.DoesNotContain(allAfterDelete, e => e.Id == enclosure.Id);
        }

        [Fact]
        public async Task FeedingScheduleRepository_CRUD_Works()
        {
            var repo = new FeedingScheduleRepository();
            var enclosure = new Enclosure(EnclosureType.Herbivore, 2);
            var animal = new Animal("Elephant", "Dumbo", DateTime.Now, Gender.Male, "Fruit", enclosure);
            var schedule = new FeedingSchedule(animal, DateTime.Now, "Fruit");

            // Add
            await repo.AddAsync(schedule);
            var all = await repo.GetAllAsync();
            Assert.Contains(all, s => s.Id == schedule.Id);

            // GetById
            var byId = await repo.GetByIdAsync(schedule.Id.ToString());
            Assert.Equal(schedule.Id, byId.Id);

            // Update (ничего не делает, но не должно падать)
            await repo.UpdateAsync(schedule);

            // Delete
            await repo.DeleteAsync(schedule.Id.ToString());
            var allAfterDelete = await repo.GetAllAsync();
            Assert.DoesNotContain(allAfterDelete, s => s.Id == schedule.Id);
        }
    }
}
