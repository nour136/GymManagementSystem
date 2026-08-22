using GymManagement.BLL.DTOs;
using GymManagement.BLL.Exceptions;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Enums;
using GymManagement.DAL.Repositories;

namespace GymManagement.BLL.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResultDto<SubscriptionDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var (subscriptions, totalCount) = await _unitOfWork.Subscriptions.GetPagedAsync(pageNumber, pageSize);
            var subscriptionList = subscriptions.ToList();

            var memberIds = subscriptionList.Select(s => s.MemberId).Distinct().ToList();
            var planIds = subscriptionList.Select(s => s.PlanId).Distinct().ToList();

            var members = (await _unitOfWork.Members.FindAsync(m => memberIds.Contains(m.Id)))
                .ToDictionary(m => m.Id, m => m.FullName);

            var plans = (await _unitOfWork.Plans.FindAsync(p => planIds.Contains(p.Id)))
                .ToDictionary(p => p.Id, p => p.Name);

            var dtos = subscriptionList.Select(s => MapToDto(
                s,
                members.GetValueOrDefault(s.MemberId, string.Empty),
                plans.GetValueOrDefault(s.PlanId, string.Empty)));

            return new PagedResultDto<SubscriptionDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<SubscriptionDto?> GetByIdAsync(int id)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(id);
            if (subscription is null)
            {
                return null;
            }

            var member = await _unitOfWork.Members.GetByIdAsync(subscription.MemberId);
            var plan = await _unitOfWork.Plans.GetByIdAsync(subscription.PlanId);

            return MapToDto(subscription, member?.FullName ?? string.Empty, plan?.Name ?? string.Empty);
        }

        public async Task<SubscriptionDto> CreateAsync(CreateSubscriptionDto dto)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId);
            if (member is null)
            {
                throw new BusinessRuleException("Member not found.");
            }

            var plan = await _unitOfWork.Plans.GetByIdAsync(dto.PlanId);
            if (plan is null)
            {
                throw new BusinessRuleException("Plan not found.");
            }

            var memberSubscriptions = await _unitOfWork.Subscriptions.FindAsync(s => s.MemberId == dto.MemberId);
            var hasActiveSubscription = memberSubscriptions.Any(IsEffectivelyActive);
            if (hasActiveSubscription)
            {
                throw new BusinessRuleException("Member already has an active subscription.");
            }

            var startDate = dto.StartDate ?? DateTime.UtcNow;
            var endDate = startDate.AddDays(plan.DurationInDays);

            var subscription = new Subscription
            {
                MemberId = dto.MemberId,
                PlanId = dto.PlanId,
                StartDate = startDate,
                EndDate = endDate,
                Status = SubscriptionStatus.Active
            };

            await _unitOfWork.Subscriptions.AddAsync(subscription);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(subscription, member.FullName, plan.Name);
        }

        public async Task<bool> CancelAsync(int id)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(id);
            if (subscription is null)
            {
                return false;
            }

            subscription.Status = SubscriptionStatus.Cancelled;

            _unitOfWork.Subscriptions.Update(subscription);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static bool IsEffectivelyActive(Subscription subscription)
        {
            return subscription.Status == SubscriptionStatus.Active && subscription.EndDate >= DateTime.UtcNow;
        }

        private static string ComputeDisplayStatus(Subscription subscription)
        {
            if (subscription.Status == SubscriptionStatus.Cancelled)
            {
                return SubscriptionStatus.Cancelled.ToString();
            }

            if (subscription.EndDate < DateTime.UtcNow)
            {
                return SubscriptionStatus.Expired.ToString();
            }

            return subscription.Status.ToString();
        }

        private static SubscriptionDto MapToDto(Subscription subscription, string memberName, string planName)
        {
            return new SubscriptionDto
            {
                Id = subscription.Id,
                MemberId = subscription.MemberId,
                MemberName = memberName,
                PlanId = subscription.PlanId,
                PlanName = planName,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                Status = ComputeDisplayStatus(subscription)
            };
        }
    }
}
