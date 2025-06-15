using Xunit;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public class AnimalTransferServiceTests
    {
        private readonly Mock<IAnimalRepository> _animalRepoMock;
        private readonly Mock<IEnclosureRepository> _enclosureRepoMock;
        private readonly AnimalTransferService _service;

        public AnimalTransferServiceTests()
        {
            _animalRepoMock = new Mock<IAnimalRepository>();
            _enclosureRepoMock = new Mock<IEnclosureRepository>();
            _service = new AnimalTransferService(_animalRepoMock.Object, _enclosureRepoMock.Object);
        }

        [Fact]
        public async Task TransferAnimal_ValidTransfer_ShouldSucceed()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var fromId = Guid.NewGuid();
            var toId = Guid.NewGuid();

            var animal = new Animal("Lion", "Simba", DateTime.Now, Gender.Male, "Meat", 
                new Enclosure(EnclosureType.Predator, 5));
            var fromEnclosure = new Enclosure(EnclosureType.Predator, 5);
            var toEnclosure = new Enclosure(EnclosureType.Predator, 5);

            _animalRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(animal);
            _enclosureRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => id == fromId.ToString() ? fromEnclosure : toEnclosure);

            // Act
            await _service.TransferAnimalAsync(animalId, fromId, toId);

            // Assert
            _animalRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Animal>()), Times.Once);
            _enclosureRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Enclosure>()), Times.Exactly(2));
        }
    }
}
