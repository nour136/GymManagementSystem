using GymManagement.BLL.DTOs;
using GymManagement.BLL.Exceptions;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Enums;
using GymManagement.DAL.Repositories;

namespace GymManagement.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResultDto<PaymentDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var (payments, totalCount) = await _unitOfWork.Payments.GetPagedAsync(pageNumber, pageSize);
            var paymentList = payments.ToList();

            var memberIds = paymentList.Select(p => p.MemberId).Distinct().ToList();
            var members = (await _unitOfWork.Members.FindAsync(m => memberIds.Contains(m.Id)))
                .ToDictionary(m => m.Id, m => m.FullName);

            var dtos = paymentList.Select(p => MapToDto(p, members.GetValueOrDefault(p.MemberId, string.Empty)));

            return new PagedResultDto<PaymentDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PaymentDto?> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (payment is null)
            {
                return null;
            }

            var member = await _unitOfWork.Members.GetByIdAsync(payment.MemberId);
            return MapToDto(payment, member?.FullName ?? string.Empty);
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId);
            if (member is null)
            {
                throw new BusinessRuleException("Member not found.");
            }

            if (dto.SubscriptionId.HasValue)
            {
                var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(dto.SubscriptionId.Value);
                if (subscription is null)
                {
                    throw new BusinessRuleException("Subscription not found.");
                }
            }

            var payment = new Payment
            {
                MemberId = dto.MemberId,
                SubscriptionId = dto.SubscriptionId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                Method = dto.Method,
                Status = PaymentStatus.Completed
            };

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(payment, member.FullName);
        }

        private static PaymentDto MapToDto(Payment payment, string memberName)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                MemberId = payment.MemberId,
                MemberName = memberName,
                SubscriptionId = payment.SubscriptionId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                Method = payment.Method.ToString(),
                Status = payment.Status.ToString()
            };
        }
    }
}
