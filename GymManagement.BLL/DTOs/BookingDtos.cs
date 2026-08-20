using System.ComponentModel.DataAnnotations;

namespace GymManagement.BLL.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public int SessionId { get; set; }
        public string SessionName { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateBookingDto
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public int SessionId { get; set; }
    }
}
