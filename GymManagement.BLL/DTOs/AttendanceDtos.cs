namespace GymManagement.BLL.DTOs
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string SessionName { get; set; } = string.Empty;
        public DateTime? CheckInTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
