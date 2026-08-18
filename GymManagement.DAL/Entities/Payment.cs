using GymManagement.DAL.Enums;

namespace GymManagement.DAL.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int? SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public PaymentMethod Method { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    }
}
