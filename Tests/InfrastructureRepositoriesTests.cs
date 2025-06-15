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

            await repo.AddAsync(animal);
            var all = await repo.GetAllAsync();
            Assert.Contains(all, a => a.Id == animal.Id);

            var byId = await repo.GetByIdAsync(animal.Id.ToString());
            Assert.Equal(animal.Id, byId.Id);

            var updatedAnimal = new Animal("Lion", "Simba", animal.BirthDate, Gender.Male, "Meat", enclosure);
            typeof(Animal).GetProperty("Id").SetValue(updatedAnimal, animal.Id); // set same Id
            await repo.UpdateAsync(updatedAnimal);
            var byIdAfterUpdate = await repo.GetByIdAsync(animal.Id.ToString());
            Assert.Equal(updatedAnimal.Name, byIdAfterUpdate.Name);

            var animalsInEnclosure = await repo.GetAnimalsByEnclosureAsync(enclosure.Id.ToString());
            Assert.Contains(animalsInEnclosure, a => a.Id == animal.Id);

            await repo.DeleteAsync(animal.Id.ToString());
            var allAfterDelete = await repo.GetAllAsync();
            Assert.DoesNotContain(allAfterDelete, a => a.Id == animal.Id);
        }

        [Fact]
        public async Task EnclosureRepository_CRUD_Works()
        {
            var repo = new EnclosureRepository();
            var enclosure = new Enclosure(EnclosureType.Predator, 2);

            await repo.AddAsync(enclosure);
            var all = await repo.GetAllAsync();
            Assert.Contains(all, e => e.Id == enclosure.Id);

            var byId = await repo.GetByIdAsync(enclosure.Id.ToString());
            Assert.Equal(enclosure.Id, byId.Id);

            var updatedEnclosure = new Enclosure(EnclosureType.Predator, 3);
            typeof(Enclosure).GetProperty("Id").SetValue(updatedEnclosure, enclosure.Id);
            await repo.UpdateAsync(updatedEnclosure);
            var byIdAfterUpdate = await repo.GetByIdAsync(enclosure.Id.ToString());
            Assert.Equal(3, byIdAfterUpdate.Capacity);

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

            await repo.AddAsync(schedule);
            var all = await repo.GetAllAsync();
            Assert.Contains(all, s => s.Id == schedule.Id);

            var byId = await repo.GetByIdAsync(schedule.Id.ToString());
            Assert.Equal(schedule.Id, byId.Id);

            await repo.UpdateAsync(schedule);

            await repo.DeleteAsync(schedule.Id.ToString());
            var allAfterDelete = await repo.GetAllAsync();
            Assert.DoesNotContain(allAfterDelete, s => s.Id == schedule.Id);
        }
    }
}
