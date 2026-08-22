using GymManagement.BLL.DTOs;
using GymManagement.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResultDto<TrainerDto>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var trainers = await _trainerService.GetAllAsync(pageNumber, pageSize);
            return Ok(trainers);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TrainerDto>> GetById(int id)
        {
            var trainer = await _trainerService.GetByIdAsync(id);
            if (trainer is null)
            {
                return NotFound();
            }

            return Ok(trainer);
        }

        [HttpPost]
        public async Task<ActionResult<TrainerDto>> Create(CreateTrainerDto dto)
        {
            var created = await _trainerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTrainerDto dto)
        {
            var updated = await _trainerService.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var deactivated = await _trainerService.DeactivateAsync(id);
            if (!deactivated)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
