using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Presentation.DTOs
{
    public class AddEnclosureRequest
    {
        [Required]
        [EnumDataType(typeof(EnclosureType))]
        public required string Type { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }
    }

    public class EnclosureResponse
    {
        public required string Id { get; set; }
        public required string Type { get; set; }
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }
    }
} 