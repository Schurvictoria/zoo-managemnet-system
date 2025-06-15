using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class AddFeedingRequest
    {
        [Required]
        public required string AnimalId { get; set; }

        [Required]
        [StringLength(50)]
        public required string FoodType { get; set; }

        [Required]
        public DateTime FeedingTime { get; set; }
    }

    public class FeedingScheduleResponse
    {
        public required string Id { get; set; }
        public required string AnimalId { get; set; }
        public required string AnimalName { get; set; }
        public required string FoodType { get; set; }
        public DateTime FeedingTime { get; set; }
    }
} 