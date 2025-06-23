using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagerApp.Models
{
    [Table("booking")]
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }

        [ForeignKey("Venue")]
        public int VenueId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        public Event? Event { get; set; }
        public Venue? Venue { get; set; }
    }
}
