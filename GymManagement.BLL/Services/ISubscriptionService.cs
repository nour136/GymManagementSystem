using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface ISubscriptionService
    {
        Task<PagedResultDto<SubscriptionDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<SubscriptionDto?> GetByIdAsync(int id);
        Task<SubscriptionDto> CreateAsync(CreateSubscriptionDto dto);
        Task<bool> CancelAsync(int id);
    }
}
