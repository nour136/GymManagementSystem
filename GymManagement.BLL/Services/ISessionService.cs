using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface ISessionService
    {
        Task<PagedResultDto<SessionDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<SessionDto?> GetByIdAsync(int id);
        Task<SessionDto> CreateAsync(CreateSessionDto dto);
        Task<bool> UpdateAsync(int id, UpdateSessionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
