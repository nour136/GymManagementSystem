using GymManagement.BLL.DTOs;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.BLL.Services
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IEnumerable<MemberDto>> GetAllAsync()
        {
            var members = (await _unitOfWork.Members.GetAllAsync()).ToList();

            var userIds = members.Select(m => m.ApplicationUserId).ToList();
            var users = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Email ?? string.Empty);

            return members.Select(m => MapToDto(m, users.GetValueOrDefault(m.ApplicationUserId, string.Empty)));
        }

        public async Task<MemberDto?> GetByIdAsync(int id)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member is null)
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(member.ApplicationUserId);
            return MapToDto(member, user?.Email ?? string.Empty);
        }

        public async Task<MemberDto> CreateAsync(CreateMemberDto dto)
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
                throw new InvalidOperationException($"Could not create user account: {errors}");
            }

            await _userManager.AddToRoleAsync(user, "Member");

            var member = new Member
            {
                ApplicationUserId = user.Id,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                JoinDate = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Members.AddAsync(member);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(member, user.Email ?? string.Empty);
        }

        public async Task<bool> UpdateAsync(int id, UpdateMemberDto dto)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member is null)
            {
                return false;
            }

            member.FullName = dto.FullName;
            member.PhoneNumber = dto.PhoneNumber;
            member.DateOfBirth = dto.DateOfBirth;

            _unitOfWork.Members.Update(member);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member is null)
            {
                return false;
            }

            member.IsActive = false;

            _unitOfWork.Members.Update(member);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static MemberDto MapToDto(Member member, string email)
        {
            return new MemberDto
            {
                Id = member.Id,
                ApplicationUserId = member.ApplicationUserId,
                FullName = member.FullName,
                Email = email,
                PhoneNumber = member.PhoneNumber,
                DateOfBirth = member.DateOfBirth,
                JoinDate = member.JoinDate,
                IsActive = member.IsActive
            };
        }
    }
}