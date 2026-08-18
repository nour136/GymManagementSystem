using System.ComponentModel.DataAnnotations;

namespace GymManagement.DAL.Entities
{
    public class Plan
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int DurationInDays { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
