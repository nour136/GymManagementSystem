using GymManagement.DAL.Entities;

namespace GymManagement.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Member> Members { get; }
        IGenericRepository<Trainer> Trainers { get; }
        IGenericRepository<Plan> Plans { get; }
        IGenericRepository<Subscription> Subscriptions { get; }
        IGenericRepository<Session> Sessions { get; }
        IGenericRepository<Booking> Bookings { get; }
        IGenericRepository<Attendance> Attendances { get; }
        IGenericRepository<Payment> Payments { get; }

        Task<int> SaveChangesAsync();
    }
}
