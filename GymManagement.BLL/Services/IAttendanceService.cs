using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDto>> GetAllAsync();
        Task<AttendanceDto?> GetByIdAsync(int id);
        Task<AttendanceDto> CheckInAsync(int bookingId);
    }
}
