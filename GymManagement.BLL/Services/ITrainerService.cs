using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface ITrainerService
    {
        Task<PagedResultDto<TrainerDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<TrainerDto?> GetByIdAsync(int id);
        Task<TrainerDto> CreateAsync(CreateTrainerDto dto);
        Task<bool> UpdateAsync(int id, UpdateTrainerDto dto);
        Task<bool> DeactivateAsync(int id);
    }
}
