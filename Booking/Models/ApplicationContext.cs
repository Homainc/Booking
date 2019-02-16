using Microsoft.EntityFrameworkCore;

namespace Booking.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Reserve> Reserves { get; set; }
        public DbSet<ReserveTeam> ReserveTeams { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Database initilization
            var userRole = new Role { Id = 1, Name = "user" };
            var adminRole = new Role { Id = 2, Name = "admin" };
            var managerRole = new Role { Id = 3, Name = "manager" };

            // Database configuration
            modelBuilder.Entity<Role>()
                .HasData(new Role[] { userRole, adminRole, managerRole });
            modelBuilder.Entity<User>()
                .Property(x => x.RoleId)
                .HasDefaultValue(userRole.Id);
            modelBuilder.Entity<RoomDevice>()
                .HasKey(rd => new { rd.DeviceId, rd.RoomId });
            modelBuilder.Entity<RoomDevice>()
                .HasOne(rd => rd.Room)
                .WithMany(d => d.RoomDevices)
                .HasForeignKey(rd => rd.DeviceId);
            modelBuilder.Entity<RoomDevice>()
                .HasOne(rd => rd.Device)
                .WithMany(r => r.RoomDevices)
                .HasForeignKey(rd => rd.RoomId);
            modelBuilder.Entity<User>()
                .HasAlternateKey(u => u.Email);
            base.OnModelCreating(modelBuilder);
        }
    }
}
