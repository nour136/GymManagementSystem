using GymManagement.DAL.Enums;

namespace GymManagement.DAL.Entities
{
    public class Subscription : IEntity
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
