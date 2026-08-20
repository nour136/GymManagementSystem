using GymManagement.DAL.Entities;
using GymManagement.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasIndex(b => new { b.MemberId, b.SessionId })
                .IsUnique()
                .HasFilter($"[Status] <> {(int)BookingStatus.Cancelled}");
        }
    }
}
