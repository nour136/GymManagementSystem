using System.ComponentModel.DataAnnotations;

namespace GymManagement.DAL.Entities
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
