using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class UserContext : IdentityDbContext<User>
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Database configuration
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
