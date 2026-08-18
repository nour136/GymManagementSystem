using GymManagement.DAL.Enums;

namespace GymManagement.DAL.Entities
{
    public class Attendance
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public DateTime? CheckInTime { get; set; }

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Absent;
    }
}
