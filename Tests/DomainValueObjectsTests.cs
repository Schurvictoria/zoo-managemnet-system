using Xunit;
using Domain.ValueObjects;

namespace Tests
{
    public class DomainValueObjectsTests
    {
        [Fact]
        public void AnimalName_Equality_Works()
        {
            var name1 = new AnimalName("Simba");
            var name2 = new AnimalName("Simba");
            var name3 = new AnimalName("Nala");

            Assert.Equal(name1, name2);
            Assert.NotEqual(name1, name3);
            Assert.Equal("Simba", name1.Value);
            Assert.Equal("Simba", name1.ToString());
        }

        [Fact]
        public void FoodType_Equality_Works()
        {
            var food1 = new FoodType("Meat");
            var food2 = new FoodType("Meat");
            var food3 = new FoodType("Grass");

            Assert.Equal(food1, food2);
            Assert.NotEqual(food1, food3);
            Assert.Equal("Meat", food1.Value);
            Assert.Equal("Meat", food1.ToString());
        }
    }
}
