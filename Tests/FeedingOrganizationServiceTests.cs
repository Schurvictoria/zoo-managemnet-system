using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;
using mini_hw_2.Services;
using mini_hw_2.Interfaces;
using Moq;

namespace Tests
{
    public class FeedingOrganizationServiceTests
    {
        [Fact]
        public void Should_Feed_Animal_According_To_Schedule()
        {
            // Arrange
            var animal = new Animal("Elephant", "Dumbo", DateTime.Now.AddYears(-5), Gender.Male, "Fruit");
            var schedule = new FeedingSchedule(animal, DateTime.Now, "Fruit");

            var mockRepo = new Mock<IFeedingScheduleRepository>();
            mockRepo.Setup(repo => repo.GetById(schedule.Id)).ReturnsAsync(schedule);

            var service = new FeedingOrganizationService(mockRepo.Object);

            // Act
            service.FeedAnimal(schedule.Id);

            // Assert
            mockRepo.Verify(repo => repo.Update(schedule), Times.Once);
        }
    }
}
