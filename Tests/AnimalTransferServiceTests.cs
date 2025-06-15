using Xunit;
using Moq;
using Application.Services;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public class AnimalTransferServiceTests
    {
        private readonly Mock<IAnimalRepository> _animalRepoMock = new();
        private readonly Mock<IEnclosureRepository> _enclosureRepoMock = new();
        private readonly Mock<ILoggingService> _loggerMock = new();
        private readonly AnimalTransferService _service;

        public AnimalTransferServiceTests()
        {
            _service = new AnimalTransferService(_animalRepoMock.Object, _enclosureRepoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task TransferAnimal_ValidTransfer_ShouldSucceed()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var fromId = Guid.NewGuid();
            var toId = Guid.NewGuid();
            var fromEnclosure = new Enclosure(EnclosureType.Predator, 5);
            var toEnclosure = new Enclosure(EnclosureType.Predator, 5);
            var animal = new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", fromEnclosure);
            fromEnclosure.AddAnimal(animal);

            _animalRepoMock.Setup(x => x.GetByIdAsync(animalId.ToString())).ReturnsAsync(animal);
            _enclosureRepoMock.Setup(x => x.GetByIdAsync(fromId.ToString())).ReturnsAsync(fromEnclosure);
            _enclosureRepoMock.Setup(x => x.GetByIdAsync(toId.ToString())).ReturnsAsync(toEnclosure);

            // Act
            await _service.TransferAnimalAsync(animalId, fromId, toId);

            // Assert
            _animalRepoMock.Verify(x => x.UpdateAsync(animal), Times.Once);
            _enclosureRepoMock.Verify(x => x.UpdateAsync(fromEnclosure), Times.Once);
            _enclosureRepoMock.Verify(x => x.UpdateAsync(toEnclosure), Times.Once);
        }

        [Fact]
        public async Task TransferAnimal_AnimalNotFound_ShouldThrow()
        {
            var animalId = Guid.NewGuid();
            var fromId = Guid.NewGuid();
            var toId = Guid.NewGuid();
            _animalRepoMock.Setup(x => x.GetByIdAsync(animalId.ToString())).ReturnsAsync((Animal)null);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.TransferAnimalAsync(animalId, fromId, toId));
        }

        [Fact]
        public async Task TransferAnimal_FromEnclosureNotFound_ShouldThrow()
        {
            var animalId = Guid.NewGuid();
            var fromId = Guid.NewGuid();
            var toId = Guid.NewGuid();
            var animal = new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", null);
            _animalRepoMock.Setup(x => x.GetByIdAsync(animalId.ToString())).ReturnsAsync(animal);
            _enclosureRepoMock.Setup(x => x.GetByIdAsync(fromId.ToString())).ReturnsAsync((Enclosure)null);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.TransferAnimalAsync(animalId, fromId, toId));
        }

        [Fact]
        public async Task TransferAnimal_ToEnclosureNotFound_ShouldThrow()
        {
            var animalId = Guid.NewGuid();
            var fromId = Guid.NewGuid();
            var toId = Guid.NewGuid();
            var fromEnclosure = new Enclosure(EnclosureType.Predator, 5);
            var animal = new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", fromEnclosure);
            _animalRepoMock.Setup(x => x.GetByIdAsync(animalId.ToString())).ReturnsAsync(animal);
            _enclosureRepoMock.Setup(x => x.GetByIdAsync(fromId.ToString())).ReturnsAsync(fromEnclosure);
            _enclosureRepoMock.Setup(x => x.GetByIdAsync(toId.ToString())).ReturnsAsync((Enclosure)null);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.TransferAnimalAsync(animalId, fromId, toId));
        }

        [Fact]
        public async Task TransferAnimal_AnimalNotInFromEnclosure_ShouldThrow()
        {
            var animalId = Guid.NewGuid();
            var fromId = Guid.NewGuid();
            var toId = Guid.NewGuid();
            var fromEnclosure = new Enclosure(EnclosureType.Predator, 5);
            var animal = new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", null);
            _animalRepoMock.Setup(x => x.GetByIdAsync(animalId.ToString())).ReturnsAsync(animal);
            _enclosureRepoMock.Setup(x => x.GetByIdAsync(fromId.ToString())).ReturnsAsync(fromEnclosure);
            _enclosureRepoMock.Setup(x => x.GetByIdAsync(toId.ToString())).ReturnsAsync(new Enclosure(EnclosureType.Predator, 5));
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.TransferAnimalAsync(animalId, fromId, toId));
        }

        [Fact]
        public async Task TransferAnimal_ToEnclosureFull_ShouldThrow()
        {
            var animalId = Guid.NewGuid();
            var fromId = Guid.NewGuid();
            var toId = Guid.NewGuid();
            var fromEnclosure = new Enclosure(EnclosureType.Predator, 5);
            var toEnclosure = new Enclosure(EnclosureType.Predator, 0); // full
            var animal = new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", fromEnclosure);
            fromEnclosure.AddAnimal(animal);
            _animalRepoMock.Setup(x => x.GetByIdAsync(animalId.ToString())).ReturnsAsync(animal);
            _enclosureRepoMock.Setup(x => x.GetByIdAsync(fromId.ToString())).ReturnsAsync(fromEnclosure);
            _enclosureRepoMock.Setup(x => x.GetByIdAsync(toId.ToString())).ReturnsAsync(toEnclosure);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.TransferAnimalAsync(animalId, fromId, toId));
        }
    }
}
