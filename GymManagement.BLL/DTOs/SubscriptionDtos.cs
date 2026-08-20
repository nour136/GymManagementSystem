using System.ComponentModel.DataAnnotations;

namespace GymManagement.BLL.DTOs
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public int PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateSubscriptionDto
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public int PlanId { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
