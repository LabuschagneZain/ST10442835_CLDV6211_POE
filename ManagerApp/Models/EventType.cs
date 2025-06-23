using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerApp.Models
{
    [Table("EventType")]
    public class EventType
    {
        [Key]
        public int EventTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("EventType")]   // Map to DB column "EventType"
        public string? TypeName { get; set; }  // Use a different property name here

        public List<Event> Events { get; set; } = new List<Event>();
    }
}
