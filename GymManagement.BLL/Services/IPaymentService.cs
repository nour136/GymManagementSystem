using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<PaymentDto?> GetByIdAsync(int id);
        Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
    }
}
