using GymManagement.BLL.DTOs;
using GymManagement.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainerDto>>> GetAll()
        {
            var trainers = await _trainerService.GetAllAsync();
            return Ok(trainers);
        }

        [HttpGet("{id}")]
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
            try
            {
                var created = await _trainerService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
