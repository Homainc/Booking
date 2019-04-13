using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Booking.Models
{
    public class UserContext : IdentityDbContext<User>
    {
        public DbSet<Building> Building { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Reserve> Reserve { get; set; }
        public DbSet<ReserveTeam> ReserveTeam { get; set; }

        public UserContext(DbContextOptions<UserContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Database configuration
            // Alternate key for User email
            modelBuilder.Entity<User>()
                .HasAlternateKey(u => u.Email);

            //Many-to-many for ReserveTeam-User
            modelBuilder.Entity<ReserveTeamUser>()
                .HasKey(rtu => new { rtu.UserId, rtu.ReserveTeamId });
            modelBuilder.Entity<ReserveTeamUser>()
                .HasOne(rtu => rtu.User)
                .WithMany(u => u.ReserveTeamUser)
                .HasForeignKey(rtu => rtu.UserId);
            modelBuilder.Entity<ReserveTeamUser>()
                .HasOne(rtu => rtu.ReserveTeam)
                .WithMany(rt => rt.ReserveTeamUser)
                .HasForeignKey(rtu => rtu.ReserveTeamId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
