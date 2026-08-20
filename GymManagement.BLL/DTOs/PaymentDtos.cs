using System.ComponentModel.DataAnnotations;
using GymManagement.DAL.Enums;

namespace GymManagement.BLL.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public int? SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class CreatePaymentDto
    {
        [Required]
        public int MemberId { get; set; }

        public int? SubscriptionId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }
    }
}
