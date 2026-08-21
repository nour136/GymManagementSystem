using GymManagement.DAL.Entities;

namespace GymManagement.BLL.Services
{
    public interface ITokenService
    {
        Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(ApplicationUser user);
    }
}
