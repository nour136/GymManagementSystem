using GymManagement.BLL.DTOs;
using GymManagement.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Trainer")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetAll()
        {
            var attendances = await _attendanceService.GetAllAsync();
            return Ok(attendances);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceDto>> GetById(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance is null)
            {
                return NotFound();
            }

            return Ok(attendance);
        }

        [HttpPost("{bookingId}/check-in")]
        public async Task<ActionResult<AttendanceDto>> CheckIn(int bookingId)
        {
            try
            {
                var created = await _attendanceService.CheckInAsync(bookingId);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
