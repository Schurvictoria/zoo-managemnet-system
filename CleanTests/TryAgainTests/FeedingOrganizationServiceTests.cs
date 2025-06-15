using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using ZooManagement.Domain.Entities;
using ZooManagement.Domain.ValueObjects;
using Application.Services;
using Application.Interfaces;

namespace CleanTests.TryAgainTests
{
    public class FeedingOrganizationServiceTests
    {
        [Fact]
        public async Task FeedAnimal_Success_When_ScheduleExists_And_NotFed()
        {
            // Arrange
            var animal = new Animal(new AnimalName("Слон"), "Loxodonta", new Enclosure("Savannah", "Травоядные", 5));
            var schedule = new FeedingSchedule(animal, FoodType.Herbivore, DateTime.UtcNow.AddHours(1));
            var repo = new Mock<IFeedingScheduleRepository>();
            repo.Setup(r => r.GetByIdAsync(schedule.Id)).ReturnsAsync(schedule);
            var service = new FeedingOrganizationService(repo.Object);

            // Act
            await service.FeedAnimal(schedule.Id);

            // Assert
            Assert.True(schedule.IsCompleted);
        }

        [Fact]
        public async Task FeedAnimal_DoesNothing_If_AlreadyFed()
        {
            // Arrange
            var animal = new Animal(new AnimalName("Слон"), "Loxodonta", new Enclosure("Savannah", "Травоядные", 5));
            var schedule = new FeedingSchedule(animal, FoodType.Herbivore, DateTime.UtcNow.AddHours(1));
            schedule.MarkAsFed();
            var repo = new Mock<IFeedingScheduleRepository>();
            repo.Setup(r => r.GetByIdAsync(schedule.Id)).ReturnsAsync(schedule);
            var service = new FeedingOrganizationService(repo.Object);

            // Act
            await service.FeedAnimal(schedule.Id);

            // Assert
            Assert.True(schedule.IsCompleted); // Было накормлено заранее
        }

        [Fact]
        public async Task FeedAnimal_Throws_If_ScheduleNotFound()
        {
            // Arrange
            var repo = new Mock<IFeedingScheduleRepository>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((FeedingSchedule)null);
            var service = new FeedingOrganizationService(repo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.FeedAnimal(Guid.NewGuid()));
        }
    }
}
