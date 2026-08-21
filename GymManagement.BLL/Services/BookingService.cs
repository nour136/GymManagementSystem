using GymManagement.BLL.DTOs;
using GymManagement.BLL.Exceptions;
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

        public async Task<IEnumerable<BookingDto>> GetAllAsync(string requestingUserId, bool isPrivileged)
        {
            IEnumerable<Booking> bookings;

            if (isPrivileged)
            {
                bookings = await _unitOfWork.Bookings.GetAllAsync();
            }
            else
            {
                var memberId = await GetMemberIdForUserAsync(requestingUserId);
                if (memberId is null)
                {
                    return Enumerable.Empty<BookingDto>();
                }

                bookings = await _unitOfWork.Bookings.FindAsync(b => b.MemberId == memberId.Value);
            }

            var bookingList = bookings.ToList();

            var memberIds = bookingList.Select(b => b.MemberId).Distinct().ToList();
            var sessionIds = bookingList.Select(b => b.SessionId).Distinct().ToList();

            var members = (await _unitOfWork.Members.FindAsync(m => memberIds.Contains(m.Id)))
                .ToDictionary(m => m.Id, m => m.FullName);

            var sessions = (await _unitOfWork.Sessions.FindAsync(s => sessionIds.Contains(s.Id)))
                .ToDictionary(s => s.Id, s => s.Name);

            return bookingList.Select(b => MapToDto(
                b,
                members.GetValueOrDefault(b.MemberId, string.Empty),
                sessions.GetValueOrDefault(b.SessionId, string.Empty)));
        }

        public async Task<BookingDto?> GetByIdAsync(int id, string requestingUserId, bool isPrivileged)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking is null)
            {
                return null;
            }

            if (!isPrivileged)
            {
                var memberId = await GetMemberIdForUserAsync(requestingUserId);
                if (memberId is null || booking.MemberId != memberId.Value)
                {
                    return null;
                }
            }

            var member = await _unitOfWork.Members.GetByIdAsync(booking.MemberId);
            var session = await _unitOfWork.Sessions.GetByIdAsync(booking.SessionId);

            return MapToDto(booking, member?.FullName ?? string.Empty, session?.Name ?? string.Empty);
        }

        public async Task<BookingDto> CreateAsync(CreateBookingDto dto, string requestingUserId, bool isAdmin)
        {
            int memberIdToUse;

            if (isAdmin)
            {
                memberIdToUse = dto.MemberId;
            }
            else
            {
                var memberId = await GetMemberIdForUserAsync(requestingUserId);
                if (memberId is null)
                {
                    throw new BusinessRuleException("No member profile found for this account.");
                }

                memberIdToUse = memberId.Value;
            }

            var member = await _unitOfWork.Members.GetByIdAsync(memberIdToUse);
            if (member is null)
            {
                throw new BusinessRuleException("Member not found.");
            }

            var session = await _unitOfWork.Sessions.GetByIdAsync(dto.SessionId);
            if (session is null)
            {
                throw new BusinessRuleException("Session not found.");
            }

            if (session.ScheduledAt <= DateTime.UtcNow)
            {
                throw new BusinessRuleException("Cannot book a session that has already started or passed.");
            }

            var sessionBookings = (await _unitOfWork.Bookings.FindAsync(b => b.SessionId == dto.SessionId)).ToList();

            var alreadyBooked = sessionBookings.Any(b =>
                b.MemberId == memberIdToUse && b.Status != BookingStatus.Cancelled);
            if (alreadyBooked)
            {
                throw new BusinessRuleException("This member already has an active booking for this session.");
            }

            var confirmedCount = sessionBookings.Count(b => b.Status == BookingStatus.Confirmed);
            if (confirmedCount >= session.Capacity)
            {
                throw new BusinessRuleException("This session is fully booked.");
            }

            var booking = new Booking
            {
                MemberId = memberIdToUse,
                SessionId = dto.SessionId,
                BookingDate = DateTime.UtcNow,
                Status = BookingStatus.Confirmed
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(booking, member.FullName, session.Name);
        }

        public async Task<bool> CancelAsync(int id, string requestingUserId, bool isPrivileged)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking is null)
            {
                return false;
            }

            if (!isPrivileged)
            {
                var memberId = await GetMemberIdForUserAsync(requestingUserId);
                if (memberId is null || booking.MemberId != memberId.Value)
                {
                    return false;
                }
            }

            booking.Status = BookingStatus.Cancelled;

            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private async Task<int?> GetMemberIdForUserAsync(string applicationUserId)
        {
            var members = await _unitOfWork.Members.FindAsync(m => m.ApplicationUserId == applicationUserId);
            return members.FirstOrDefault()?.Id;
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
