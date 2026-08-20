using GymManagement.BLL.DTOs;

namespace GymManagement.BLL.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetAllAsync();
        Task<MemberDto?> GetByIdAsync(int id);
        Task<MemberDto> CreateAsync(CreateMemberDto dto);
        Task<bool> UpdateAsync(int id, UpdateMemberDto dto);
        Task<bool> DeactivateAsync(int id);
    }
}
