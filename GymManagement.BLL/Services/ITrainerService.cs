using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerDto>> GetAllAsync();
        Task<TrainerDto?> GetByIdAsync(int id);
        Task<TrainerDto> CreateAsync(CreateTrainerDto dto);
        Task<bool> UpdateAsync(int id, UpdateTrainerDto dto);
        Task<bool> DeactivateAsync(int id);
    }
}
