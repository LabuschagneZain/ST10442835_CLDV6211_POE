using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerApp.Models
{
    [Table("Event")] // This maps to the Event table
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // Foreign Key for Venue
        [ForeignKey("Venue")]
        public int VenueId { get; set; }
        public Venue? Venue { get; set; }

        // Foreign Key for EventType
        [ForeignKey("EventType")]
        public int? EventTypeId { get; set; }
        public EventType? EventType { get; set; }

        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
