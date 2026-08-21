using GymManagement.BLL.DTOs;
using GymManagement.BLL.Services;
using GymManagement.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiresAt) = await _tokenService.GenerateTokenAsync(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Roles = roles
            });
        }
    }
}
