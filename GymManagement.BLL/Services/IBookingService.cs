using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllAsync(string requestingUserId, bool isPrivileged);
        Task<BookingDto?> GetByIdAsync(int id, string requestingUserId, bool isPrivileged);
        Task<BookingDto> CreateAsync(CreateBookingDto dto, string requestingUserId, bool isAdmin);
        Task<bool> CancelAsync(int id, string requestingUserId, bool isPrivileged);
    }
}
