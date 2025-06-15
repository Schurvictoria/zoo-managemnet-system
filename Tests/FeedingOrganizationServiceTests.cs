using System;
using Xunit;
using Moq;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;

namespace Tests
{
    public class FeedingOrganizationServiceTests
    {
        private readonly Mock<IFeedingScheduleRepository> _feedingScheduleRepoMock = new();
        private readonly FeedingOrganizationService _service;

        public FeedingOrganizationServiceTests()
        {
            _service = new FeedingOrganizationService(_feedingScheduleRepoMock.Object);
        }

        [Fact]
        public void FeedAnimal_ScheduleExists_ShouldFeed()
        {
            var animal = new Animal("Elephant", "Dumbo", DateTime.Now.AddYears(-5), Gender.Male, "Fruit", null);
            var schedule = new FeedingSchedule(animal, DateTime.Now, "Fruit");
            _feedingScheduleRepoMock.Setup(r => r.GetByIdAsync(schedule.Id.ToString())).ReturnsAsync(schedule);

            _service.FeedAnimal(schedule.Id);

            _feedingScheduleRepoMock.Verify(r => r.GetByIdAsync(schedule.Id.ToString()), Times.Once);
        }

        [Fact]
        public void FeedAnimal_ScheduleNotFound_ShouldNotThrow()
        {
            var scheduleId = Guid.NewGuid();
            _feedingScheduleRepoMock.Setup(r => r.GetByIdAsync(scheduleId.ToString())).ReturnsAsync((FeedingSchedule)null);

            var ex = Record.Exception(() => _service.FeedAnimal(scheduleId));
            Assert.Null(ex);
        }

        [Fact]
        public void FeedAnimal_AlreadyFed_ShouldNotThrow()
        {
            var animal = new Animal("Elephant", "Dumbo", DateTime.Now.AddYears(-5), Gender.Male, "Fruit", null);
            var schedule = new FeedingSchedule(animal, DateTime.Now, "Fruit");
            schedule.MarkAsCompleted();
            _feedingScheduleRepoMock.Setup(r => r.GetByIdAsync(schedule.Id.ToString())).ReturnsAsync(schedule);

            var ex = Record.Exception(() => _service.FeedAnimal(schedule.Id));

            Assert.Null(ex);
        }
    }
}
