using GymManagement.BLL.DTOs;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Enums;
using GymManagement.DAL.Repositories;

namespace GymManagement.BLL.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BookingDto>> GetAllAsync()
        {
            var bookings = (await _unitOfWork.Bookings.GetAllAsync()).ToList();

            var memberIds = bookings.Select(b => b.MemberId).Distinct().ToList();
            var sessionIds = bookings.Select(b => b.SessionId).Distinct().ToList();

            var members = (await _unitOfWork.Members.FindAsync(m => memberIds.Contains(m.Id)))
                .ToDictionary(m => m.Id, m => m.FullName);

            var sessions = (await _unitOfWork.Sessions.FindAsync(s => sessionIds.Contains(s.Id)))
                .ToDictionary(s => s.Id, s => s.Name);

            return bookings.Select(b => MapToDto(
                b,
                members.GetValueOrDefault(b.MemberId, string.Empty),
                sessions.GetValueOrDefault(b.SessionId, string.Empty)));
        }

        public async Task<BookingDto?> GetByIdAsync(int id)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking is null)
            {
                return null;
            }

            var member = await _unitOfWork.Members.GetByIdAsync(booking.MemberId);
            var session = await _unitOfWork.Sessions.GetByIdAsync(booking.SessionId);

            return MapToDto(booking, member?.FullName ?? string.Empty, session?.Name ?? string.Empty);
        }

        public async Task<BookingDto> CreateAsync(CreateBookingDto dto)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId);
            if (member is null)
            {
                throw new InvalidOperationException("Member not found.");
            }

            var session = await _unitOfWork.Sessions.GetByIdAsync(dto.SessionId);
            if (session is null)
            {
                throw new InvalidOperationException("Session not found.");
            }

            if (session.ScheduledAt <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot book a session that has already started or passed.");
            }

            var sessionBookings = (await _unitOfWork.Bookings.FindAsync(b => b.SessionId == dto.SessionId)).ToList();

            var alreadyBooked = sessionBookings.Any(b =>
                b.MemberId == dto.MemberId && b.Status != BookingStatus.Cancelled);
            if (alreadyBooked)
            {
                throw new InvalidOperationException("This member already has an active booking for this session.");
            }

            var confirmedCount = sessionBookings.Count(b => b.Status == BookingStatus.Confirmed);
            if (confirmedCount >= session.Capacity)
            {
                throw new InvalidOperationException("This session is fully booked.");
            }

            var booking = new Booking
            {
                MemberId = dto.MemberId,
                SessionId = dto.SessionId,
                BookingDate = DateTime.UtcNow,
                Status = BookingStatus.Confirmed
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(booking, member.FullName, session.Name);
        }

        public async Task<bool> CancelAsync(int id)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking is null)
            {
                return false;
            }

            booking.Status = BookingStatus.Cancelled;

            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static BookingDto MapToDto(Booking booking, string memberName, string sessionName)
        {
            return new BookingDto
            {
                Id = booking.Id,
                MemberId = booking.MemberId,
                MemberName = memberName,
                SessionId = booking.SessionId,
                SessionName = sessionName,
                BookingDate = booking.BookingDate,
                Status = booking.Status.ToString()
            };
        }
    }
}
