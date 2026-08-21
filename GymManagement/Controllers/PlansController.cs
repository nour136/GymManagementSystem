using GymManagement.BLL.DTOs;
using GymManagement.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class PlansController : ControllerBase
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PlanDto>>> GetAll()
        {
            var plans = await _planService.GetAllAsync();
            return Ok(plans);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PlanDto>> GetById(int id)
        {
            var plan = await _planService.GetByIdAsync(id);
            if (plan is null)
            {
                return NotFound();
            }

            return Ok(plan);
        }

        [HttpPost]
        public async Task<ActionResult<PlanDto>> Create(CreatePlanDto dto)
        {
            var created = await _planService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePlanDto dto)
        {
            var updated = await _planService.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _planService.DeleteAsync(id);
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
