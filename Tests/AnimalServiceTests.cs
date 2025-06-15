using Xunit;
using Moq;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class AnimalServiceTests
    {
        private readonly Mock<IAnimalRepository> _animalRepoMock = new();
        private readonly AnimalService _service;

        public AnimalServiceTests()
        {
            _service = new AnimalService(_animalRepoMock.Object);
        }

        [Fact]
        public async Task GetAllAnimalsAsync_ReturnsAnimals()
        {
            var animals = new List<Animal> { new Animal("Cat", "Tom", DateTime.Now, Gender.Male, "Fish", null) };
            _animalRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(animals);
            var result = await _service.GetAllAnimalsAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task AddAnimalAsync_AddsAnimalAndReturnsId()
        {
            var enclosure = new Enclosure(EnclosureType.Predator, 2);
            _animalRepoMock.Setup(r => r.AddAsync(It.IsAny<Animal>())).Returns(Task.CompletedTask);
            var id = await _service.AddAnimalAsync("Cat", "Tom", DateTime.Now, Gender.Male, "Fish", enclosure);
            Assert.NotEqual(Guid.Empty, id);
            _animalRepoMock.Verify(r => r.AddAsync(It.IsAny<Animal>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAnimalAsync_DeletesAnimal()
        {
            _animalRepoMock.Setup(r => r.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            await _service.DeleteAnimalAsync(Guid.NewGuid());
            _animalRepoMock.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Once);
        }
    }
} 