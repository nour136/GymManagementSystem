using System.ComponentModel.DataAnnotations;

namespace GymManagement.DAL.Entities
{
    public class Session
    {
        public int Id { get; set; }

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime ScheduledAt { get; set; }

        public int DurationMinutes { get; set; }

        public int Capacity { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
