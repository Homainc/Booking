﻿using Microsoft.EntityFrameworkCore;

namespace Booking.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
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