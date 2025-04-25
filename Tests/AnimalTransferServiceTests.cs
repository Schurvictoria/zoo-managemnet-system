using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Tests
{
    [Fact]
    public void Should_Transfer_Animal_Between_Enclosures()
    {
        var animal = new Animal("Tiger", "Shere Khan", DateTime.Now.AddYears(-3), Gender.Male, "Meat");
        var from = new Enclosure(EnclosureType.Predator, 3);
        var to = new Enclosure(EnclosureType.Predator, 3);
        from.AddAnimal(animal);

        var animalRepo = new AnimalRepository();
        var encRepo = new EnclosureRepository();
        animalRepo.Add(animal);
        encRepo.Add(from);
        encRepo.Add(to);

        var service = new AnimalTransferService(animalRepo, encRepo);

        service.TransferAnimal(animal.Id, from.Id, to.Id);

        Assert.DoesNotContain(animal, from.Animals);
        Assert.Contains(animal, to.Animals);
    }
}
