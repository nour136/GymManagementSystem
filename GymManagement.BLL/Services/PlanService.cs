using GymManagement.BLL.DTOs;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories;

namespace GymManagement.BLL.Services
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlanDto>> GetAllAsync()
        {
            var plans = await _unitOfWork.Plans.GetAllAsync();
            return plans.Select(MapToDto);
        }

        public async Task<PlanDto?> GetByIdAsync(int id)
        {
            var plan = await _unitOfWork.Plans.GetByIdAsync(id);
            return plan is null ? null : MapToDto(plan);
        }

        public async Task<PlanDto> CreateAsync(CreatePlanDto dto)
        {
            var plan = new Plan
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                DurationInDays = dto.DurationInDays
            };

            await _unitOfWork.Plans.AddAsync(plan);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(plan);
        }

        public async Task<bool> UpdateAsync(int id, UpdatePlanDto dto)
        {
            var plan = await _unitOfWork.Plans.GetByIdAsync(id);
            if (plan is null)
            {
                return false;
            }

            plan.Name = dto.Name;
            plan.Description = dto.Description;
            plan.Price = dto.Price;
            plan.DurationInDays = dto.DurationInDays;

            _unitOfWork.Plans.Update(plan);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var plan = await _unitOfWork.Plans.GetByIdAsync(id);
            if (plan is null)
            {
                return false;
            }

            var existingSubscriptions = await _unitOfWork.Subscriptions.FindAsync(s => s.PlanId == id);
            if (existingSubscriptions.Any())
            {
                throw new InvalidOperationException(
                    "Cannot delete a plan that has existing subscriptions. Deactivate it instead.");
            }

            _unitOfWork.Plans.Remove(plan);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static PlanDto MapToDto(Plan plan)
        {
            return new PlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationInDays = plan.DurationInDays
            };
        }
    }
}
