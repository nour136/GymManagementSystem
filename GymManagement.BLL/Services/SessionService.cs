using GymManagement.BLL.DTOs;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories;

namespace GymManagement.BLL.Services
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SessionDto>> GetAllAsync()
        {
            var sessions = (await _unitOfWork.Sessions.GetAllAsync()).ToList();

            var trainerIds = sessions.Select(s => s.TrainerId).Distinct().ToList();
            var trainers = (await _unitOfWork.Trainers.FindAsync(t => trainerIds.Contains(t.Id)))
                .ToDictionary(t => t.Id, t => t.FullName);

            return sessions.Select(s => MapToDto(s, trainers.GetValueOrDefault(s.TrainerId, string.Empty)));
        }

        public async Task<SessionDto?> GetByIdAsync(int id)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(id);
            if (session is null)
            {
                return null;
            }

            var trainer = await _unitOfWork.Trainers.GetByIdAsync(session.TrainerId);
            return MapToDto(session, trainer?.FullName ?? string.Empty);
        }

        public async Task<SessionDto> CreateAsync(CreateSessionDto dto)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(dto.TrainerId);
            if (trainer is null)
            {
                throw new InvalidOperationException("Trainer not found.");
            }

            if (dto.ScheduledAt <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Session cannot be scheduled in the past.");
            }

            var session = new Session
            {
                TrainerId = dto.TrainerId,
                Name = dto.Name,
                Description = dto.Description,
                ScheduledAt = dto.ScheduledAt,
                DurationMinutes = dto.DurationMinutes,
                Capacity = dto.Capacity
            };

            await _unitOfWork.Sessions.AddAsync(session);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(session, trainer.FullName);
        }

        public async Task<bool> UpdateAsync(int id, UpdateSessionDto dto)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(id);
            if (session is null)
            {
                return false;
            }

            var trainer = await _unitOfWork.Trainers.GetByIdAsync(dto.TrainerId);
            if (trainer is null)
            {
                throw new InvalidOperationException("Trainer not found.");
            }

            session.TrainerId = dto.TrainerId;
            session.Name = dto.Name;
            session.Description = dto.Description;
            session.ScheduledAt = dto.ScheduledAt;
            session.DurationMinutes = dto.DurationMinutes;
            session.Capacity = dto.Capacity;

            _unitOfWork.Sessions.Update(session);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(id);
            if (session is null)
            {
                return false;
            }

            var existingBookings = await _unitOfWork.Bookings.FindAsync(b => b.SessionId == id);
            if (existingBookings.Any())
            {
                throw new InvalidOperationException(
                    "Cannot delete a session that has existing bookings.");
            }

            _unitOfWork.Sessions.Remove(session);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static SessionDto MapToDto(Session session, string trainerName)
        {
            return new SessionDto
            {
                Id = session.Id,
                TrainerId = session.TrainerId,
                TrainerName = trainerName,
                Name = session.Name,
                Description = session.Description,
                ScheduledAt = session.ScheduledAt,
                DurationMinutes = session.DurationMinutes,
                Capacity = session.Capacity
            };
        }
    }
}
