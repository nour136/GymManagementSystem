using GymManagement.BLL.DTOs;
using GymManagement.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetAll()
        {
            var sessions = await _sessionService.GetAllAsync();
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SessionDto>> GetById(int id)
        {
            var session = await _sessionService.GetByIdAsync(id);
            if (session is null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        [HttpPost]
        public async Task<ActionResult<SessionDto>> Create(CreateSessionDto dto)
        {
            try
            {
                var created = await _sessionService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateSessionDto dto)
        {
            try
            {
                var updated = await _sessionService.UpdateAsync(id, dto);
                if (!updated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _sessionService.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
