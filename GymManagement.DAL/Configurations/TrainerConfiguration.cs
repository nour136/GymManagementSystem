using GymManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Configurations
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.HasIndex(t => t.ApplicationUserId).IsUnique();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(t => t.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
