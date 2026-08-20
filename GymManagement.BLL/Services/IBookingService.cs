using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllAsync();
        Task<BookingDto?> GetByIdAsync(int id);
        Task<BookingDto> CreateAsync(CreateBookingDto dto);
        Task<bool> CancelAsync(int id);
    }
}
