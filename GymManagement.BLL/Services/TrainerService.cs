using GymManagement.BLL.DTOs;
using GymManagement.BLL.Exceptions;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManagement.BLL.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TrainerService> _logger;

        public TrainerService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<TrainerService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<PagedResultDto<TrainerDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var (trainers, totalCount) = await _unitOfWork.Trainers.GetPagedAsync(pageNumber, pageSize);
            var trainerList = trainers.ToList();

            var userIds = trainerList.Select(t => t.ApplicationUserId).ToList();
            var users = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Email ?? string.Empty);

            var dtos = trainerList.Select(t => MapToDto(t, users.GetValueOrDefault(t.ApplicationUserId, string.Empty)));

            return new PagedResultDto<TrainerDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<TrainerDto?> GetByIdAsync(int id)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(id);
            if (trainer is null)
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(trainer.ApplicationUserId);
            return MapToDto(trainer, user?.Email ?? string.Empty);
        }

        public async Task<TrainerDto> CreateAsync(CreateTrainerDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new BusinessRuleException($"Could not create user account: {errors}");
            }

            await _userManager.AddToRoleAsync(user, "Trainer");

            var trainer = new Trainer
            {
                ApplicationUserId = user.Id,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Specialization = dto.Specialization,
                HireDate = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Trainers.AddAsync(trainer);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("New trainer added: {Email} (TrainerId: {TrainerId})", dto.Email, trainer.Id);

            return MapToDto(trainer, user.Email ?? string.Empty);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTrainerDto dto)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(id);
            if (trainer is null)
            {
                return false;
            }

            trainer.FullName = dto.FullName;
            trainer.PhoneNumber = dto.PhoneNumber;
            trainer.Specialization = dto.Specialization;

            _unitOfWork.Trainers.Update(trainer);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(id);
            if (trainer is null)
            {
                return false;
            }

            trainer.IsActive = false;

            _unitOfWork.Trainers.Update(trainer);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Trainer deactivated: TrainerId {TrainerId}", id);

            return true;
        }

        private static TrainerDto MapToDto(Trainer trainer, string email)
        {
            return new TrainerDto
            {
                Id = trainer.Id,
                ApplicationUserId = trainer.ApplicationUserId,
                FullName = trainer.FullName,
                Email = email,
                PhoneNumber = trainer.PhoneNumber,
                Specialization = trainer.Specialization,
                HireDate = trainer.HireDate,
                IsActive = trainer.IsActive
            };
        }
    }
}
