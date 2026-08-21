using System.Security.Claims;
using GymManagement.BLL.DTOs;
using GymManagement.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Trainer,Member")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<BookingDto>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var bookings = await _bookingService.GetAllAsync(pageNumber, pageSize, GetUserId(), IsPrivileged());
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id, GetUserId(), IsPrivileged());
            if (booking is null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Member")]
        public async Task<ActionResult<BookingDto>> Create(CreateBookingDto dto)
        {
            var created = await _bookingService.CreateAsync(dto, GetUserId(), User.IsInRole("Admin"));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Cancel(int id)
        {
            var cancelled = await _bookingService.CancelAsync(id, GetUserId(), User.IsInRole("Admin"));
            if (!cancelled)
            {
                return NotFound();
            }

            return NoContent();
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        private bool IsPrivileged()
        {
            return User.IsInRole("Admin") || User.IsInRole("Trainer");
        }
    }
}
