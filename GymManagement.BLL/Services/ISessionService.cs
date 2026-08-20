using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionDto>> GetAllAsync();
        Task<SessionDto?> GetByIdAsync(int id);
        Task<SessionDto> CreateAsync(CreateSessionDto dto);
        Task<bool> UpdateAsync(int id, UpdateSessionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
