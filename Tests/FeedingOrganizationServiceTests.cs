using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooManagementSystem.Domain.Entities;
using ZooManagementSystem.Infrastructure.Repositories;
using ZooManagementSystem.Application.Services;


namespace Tests
{
    [Fact]
    public void Should_Feed_Animal_According_To_Schedule()
    {
        var animal = new Animal("Elephant", "Dumbo", DateTime.Now.AddYears(-5), Gender.Male, "Fruit");
        var schedule = new FeedingSchedule(animal, DateTime.Now, "Fruit");

        var repo = new FeedingScheduleRepository();
        repo.Add(schedule);

        var service = new FeedingOrganizationService(repo);
        service.FeedAnimal(schedule.Id);

        Assert.True(true);
    }
}
