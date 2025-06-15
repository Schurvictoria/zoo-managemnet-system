using System;

namespace Domain.Entities
{
    public enum AnimalStatus { Healthy, Sick }
    public enum Gender { Male, Female }

    public class Animal
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Species { get; private set; }
        public string Name { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Gender Gender { get; private set; }
        public string FavoriteFood { get; private set; }
        public AnimalStatus Status { get; private set; } = AnimalStatus.Healthy;
        public Enclosure Enclosure { get; private set; }
        public string FoodType { get; private set; }

        public Animal(string species, string name, DateTime birthDate, Gender gender, string favoriteFood, Enclosure enclosure)
        {
            Species = species;
            Name = name;
            BirthDate = birthDate;
            Gender = gender;
            FavoriteFood = favoriteFood;
            Enclosure = enclosure;
            FoodType = favoriteFood;
        }

        public void Feed() => Console.WriteLine($"{Name} был(а) накормлен(а)");
        public void Heal() => Status = AnimalStatus.Healthy;
        public void SetSick() => Status = AnimalStatus.Sick;
        public void UpdateEnclosure(Enclosure newEnclosure) => Enclosure = newEnclosure;
    }
}
