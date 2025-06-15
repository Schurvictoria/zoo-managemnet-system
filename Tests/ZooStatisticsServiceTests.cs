using Xunit;
using Moq;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Tests
{
    public class ZooStatisticsServiceTests
    {
        private readonly Mock<IAnimalRepository> _animalRepoMock = new();
        private readonly Mock<IEnclosureRepository> _enclosureRepoMock = new();
        private readonly ZooStatisticsService _service;

        public ZooStatisticsServiceTests()
        {
            _service = new ZooStatisticsService(_animalRepoMock.Object, _enclosureRepoMock.Object);
        }

        [Fact]
        public async Task GetAnimalCountAsync_ReturnsCorrectCount()
        {
            _animalRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Animal> { new Animal("Cat", "Tom", DateTime.Now, Gender.Male, "Fish", null) });
            var count = await _service.GetAnimalCountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetFreeEnclosureCountAsync_ReturnsCorrectCount()
        {
            var enclosure1 = new Enclosure(EnclosureType.Predator, 2);
            var enclosure2 = new Enclosure(EnclosureType.Herbivore, 1);
            enclosure1.AddAnimal(new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", enclosure1));
            _enclosureRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Enclosure> { enclosure1, enclosure2 });
            var count = await _service.GetFreeEnclosureCountAsync();
            Assert.Equal(2, count); // оба не заполнены полностью
        }

        [Fact]
        public async Task TransferAnimalAsync_ThrowsIfAnimalNotFound()
        {
            _animalRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Animal)null);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.TransferAnimalAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        }

        [Fact]
        public async Task TransferAnimalAsync_ThrowsIfEnclosureNotFound()
        {
            var animal = new Animal("Cat", "Tom", DateTime.Now, Gender.Male, "Fish", null);
            _animalRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(animal);
            _enclosureRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Enclosure)null);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.TransferAnimalAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        }

        [Fact]
        public async Task TransferAnimalAsync_Success()
        {
            var from = new Enclosure(EnclosureType.Predator, 2);
            var to = new Enclosure(EnclosureType.Predator, 2);
            var animal = new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", from);
            from.AddAnimal(animal);
            _animalRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(animal);
            _enclosureRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(from);
            _enclosureRepoMock.Setup(r => r.GetByIdAsync(It.Is<string>(id => id == to.Id.ToString()))).ReturnsAsync(to);
            _animalRepoMock.Setup(r => r.UpdateAsync(animal)).Returns(Task.CompletedTask);
            _enclosureRepoMock.Setup(r => r.UpdateAsync(from)).Returns(Task.CompletedTask);
            _enclosureRepoMock.Setup(r => r.UpdateAsync(to)).Returns(Task.CompletedTask);
            await _service.TransferAnimalAsync(animal.Id, from.Id, to.Id);
            _animalRepoMock.Verify(r => r.UpdateAsync(animal), Times.Once);
        }
    }
} 