using GymManagement.DAL.Data;
using GymManagement.DAL.Entities;

namespace GymManagement.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IGenericRepository<Member>? _members;
        private IGenericRepository<Trainer>? _trainers;
        private IGenericRepository<Plan>? _plans;
        private IGenericRepository<Subscription>? _subscriptions;
        private IGenericRepository<Session>? _sessions;
        private IGenericRepository<Booking>? _bookings;
        private IGenericRepository<Attendance>? _attendances;
        private IGenericRepository<Payment>? _payments;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Member> Members =>
            _members ??= new GenericRepository<Member>(_context);

        public IGenericRepository<Trainer> Trainers =>
            _trainers ??= new GenericRepository<Trainer>(_context);

        public IGenericRepository<Plan> Plans =>
            _plans ??= new GenericRepository<Plan>(_context);

        public IGenericRepository<Subscription> Subscriptions =>
            _subscriptions ??= new GenericRepository<Subscription>(_context);

        public IGenericRepository<Session> Sessions =>
            _sessions ??= new GenericRepository<Session>(_context);

        public IGenericRepository<Booking> Bookings =>
            _bookings ??= new GenericRepository<Booking>(_context);

        public IGenericRepository<Attendance> Attendances =>
            _attendances ??= new GenericRepository<Attendance>(_context);

        public IGenericRepository<Payment> Payments =>
            _payments ??= new GenericRepository<Payment>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
