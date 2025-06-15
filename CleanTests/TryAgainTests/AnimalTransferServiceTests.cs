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
    public class AnimalTransferServiceTests
    {
        [Fact]
        public async Task TransferAnimal_Success_When_Enclosures_Compatible_And_NotFull()
        {
            // Arrange
            var animal = new Animal(new AnimalName("Лев"), "Panthera leo", new Enclosure("Savannah", "Травоядные", 5));
            var fromEnclosure = animal.Enclosure;
            var toEnclosure = new Enclosure("Savannah 2", "Травоядные", 5);

            var animalRepo = new Mock<IAnimalRepository>();
            var enclosureRepo = new Mock<IEnclosureRepository>();
            animalRepo.Setup(r => r.GetByIdAsync(animal.Id)).ReturnsAsync(animal);
            enclosureRepo.Setup(r => r.GetByIdAsync(fromEnclosure.Id)).ReturnsAsync(fromEnclosure);
            enclosureRepo.Setup(r => r.GetByIdAsync(toEnclosure.Id)).ReturnsAsync(toEnclosure);

            var service = new AnimalTransferService(animalRepo.Object, enclosureRepo.Object);

            // Act
            await service.TransferAnimal(animal.Id, fromEnclosure.Id, toEnclosure.Id);

            // Assert
            animalRepo.Verify(r => r.UpdateAsync(It.Is<Animal>(a => a.Enclosure.Id == toEnclosure.Id)), Times.Once);
        }

        [Fact]
        public async Task TransferAnimal_Throws_When_TargetEnclosureFull()
        {
            // Arrange
            var animal = new Animal(new AnimalName("Лев"), "Panthera leo", new Enclosure("Savannah", "Травоядные", 1));
            var fromEnclosure = animal.Enclosure;
            var toEnclosure = new Enclosure("Savannah 2", "Травоядные", 1);
            toEnclosure.AddAnimal(new Animal(new AnimalName("Зебра"), "Equus quagga", toEnclosure));

            var animalRepo = new Mock<IAnimalRepository>();
            var enclosureRepo = new Mock<IEnclosureRepository>();
            animalRepo.Setup(r => r.GetByIdAsync(animal.Id)).ReturnsAsync(animal);
            enclosureRepo.Setup(r => r.GetByIdAsync(fromEnclosure.Id)).ReturnsAsync(fromEnclosure);
            enclosureRepo.Setup(r => r.GetByIdAsync(toEnclosure.Id)).ReturnsAsync(toEnclosure);

            var service = new AnimalTransferService(animalRepo.Object, enclosureRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.TransferAnimal(animal.Id, fromEnclosure.Id, toEnclosure.Id));
        }

        [Fact]
        public async Task TransferAnimal_Throws_When_EnclosureTypesNotCompatible()
        {
            // Arrange
            var animal = new Animal(new AnimalName("Пингвин"), "Spheniscidae", new Enclosure("Aquarium", "Птицы", 5));
            var fromEnclosure = animal.Enclosure;
            var toEnclosure = new Enclosure("Predator", "Хищники", 5);

            var animalRepo = new Mock<IAnimalRepository>();
            var enclosureRepo = new Mock<IEnclosureRepository>();
            animalRepo.Setup(r => r.GetByIdAsync(animal.Id)).ReturnsAsync(animal);
            enclosureRepo.Setup(r => r.GetByIdAsync(fromEnclosure.Id)).ReturnsAsync(fromEnclosure);
            enclosureRepo.Setup(r => r.GetByIdAsync(toEnclosure.Id)).ReturnsAsync(toEnclosure);

            var service = new AnimalTransferService(animalRepo.Object, enclosureRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.TransferAnimal(animal.Id, fromEnclosure.Id, toEnclosure.Id));
        }
    }
}