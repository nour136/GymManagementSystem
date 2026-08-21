using GymManagement.BLL.DTOs;
using GymManagement.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Trainer")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetAll()
        {
            var subscriptions = await _subscriptionService.GetAllAsync();
            return Ok(subscriptions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionDto>> GetById(int id)
        {
            var subscription = await _subscriptionService.GetByIdAsync(id);
            if (subscription is null)
            {
                return NotFound();
            }

            return Ok(subscription);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SubscriptionDto>> Create(CreateSubscriptionDto dto)
        {
            try
            {
                var created = await _subscriptionService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int id)
        {
            var cancelled = await _subscriptionService.CancelAsync(id);
            if (!cancelled)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
