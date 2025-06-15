using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Presentation.DTOs
{
    public class AddAnimalRequest
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(100)]
        public required string Species { get; set; }

        [Required]
        [StringLength(50)]
        public required string FoodType { get; set; }

        [Required]
        public required string EnclosureId { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public required string Gender { get; set; }
    }

    public class AnimalResponse
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Species { get; set; }
        public required string FoodType { get; set; }
        public required string EnclosureId { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Gender { get; set; }
    }
} 