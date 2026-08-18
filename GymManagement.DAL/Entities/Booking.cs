using GymManagement.DAL.Enums;

namespace GymManagement.DAL.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

        public Attendance? Attendance { get; set; }
    }
}
