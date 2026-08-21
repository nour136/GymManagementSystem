using GymManagement.BLL.DTOs;
using GymManagement.BLL.Exceptions;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Enums;
using GymManagement.DAL.Repositories;

namespace GymManagement.BLL.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AttendanceDto>> GetAllAsync()
        {
            var attendances = (await _unitOfWork.Attendances.GetAllAsync()).ToList();
            var result = new List<AttendanceDto>();

            foreach (var attendance in attendances)
            {
                var dto = await BuildDtoAsync(attendance);
                result.Add(dto);
            }

            return result;
        }

        public async Task<AttendanceDto?> GetByIdAsync(int id)
        {
            var attendance = await _unitOfWork.Attendances.GetByIdAsync(id);
            if (attendance is null)
            {
                return null;
            }

            return await BuildDtoAsync(attendance);
        }

        public async Task<AttendanceDto> CheckInAsync(int bookingId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
            if (booking is null)
            {
                throw new BusinessRuleException("Booking not found.");
            }

            var existing = await _unitOfWork.Attendances.FindAsync(a => a.BookingId == bookingId);
            if (existing.Any())
            {
                throw new BusinessRuleException("This booking has already been checked in.");
            }

            var attendance = new Attendance
            {
                BookingId = bookingId,
                CheckInTime = DateTime.UtcNow,
                Status = AttendanceStatus.Present
            };

            await _unitOfWork.Attendances.AddAsync(attendance);
            await _unitOfWork.SaveChangesAsync();

            return await BuildDtoAsync(attendance);
        }

        private async Task<AttendanceDto> BuildDtoAsync(Attendance attendance)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(attendance.BookingId);

            var memberName = string.Empty;
            var sessionName = string.Empty;

            if (booking is not null)
            {
                var member = await _unitOfWork.Members.GetByIdAsync(booking.MemberId);
                var session = await _unitOfWork.Sessions.GetByIdAsync(booking.SessionId);
                memberName = member?.FullName ?? string.Empty;
                sessionName = session?.Name ?? string.Empty;
            }

            return new AttendanceDto
            {
                Id = attendance.Id,
                BookingId = attendance.BookingId,
                MemberName = memberName,
                SessionName = sessionName,
                CheckInTime = attendance.CheckInTime,
                Status = attendance.Status.ToString()
            };
        }
    }
}
