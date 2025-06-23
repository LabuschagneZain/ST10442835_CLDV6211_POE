using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ManagerApp.Models
{
    [Table("venue")]
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required]
        [MaxLength(100)]
        public string VenueName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Location { get; set; }

        [Required]
        public int Capacity { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        [Required]
        public bool Availability { get; set; } = true;

        [NotMapped]
        public IFormFile? ImageFile { get; set; } 

        public List<Event> Events { get; set; } = new List<Event>();
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
