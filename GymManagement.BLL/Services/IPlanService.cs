using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanDto>> GetAllAsync();
        Task<PlanDto?> GetByIdAsync(int id);
        Task<PlanDto> CreateAsync(CreatePlanDto dto);
        Task<bool> UpdateAsync(int id, UpdatePlanDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
