using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface IPaymentService
    {
        Task<PagedResultDto<PaymentDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<PaymentDto?> GetByIdAsync(int id);
        Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
    }
}
