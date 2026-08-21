using GymManagement.BLL.DTOs;
using GymManagement.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Trainer")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll()
        {
            var members = await _memberService.GetAllAsync();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetById(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member is null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<MemberDto>> Create(CreateMemberDto dto)
        {
            try
            {
                var created = await _memberService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateMemberDto dto)
        {
            var updated = await _memberService.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var deactivated = await _memberService.DeactivateAsync(id);
            if (!deactivated)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
