using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface IAttendanceService
    {
        Task<PagedResultDto<AttendanceDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<AttendanceDto?> GetByIdAsync(int id);
        Task<AttendanceDto> CheckInAsync(int bookingId);
    }
}
