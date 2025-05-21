using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sportics.Model.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MembershipOrder> MembershipOrders { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ClientSessionRecord> ClientSessionRecords { get; set; }
        public DbSet<CoachReview> CoachReviews { get; set; }
        public DbSet<SessionReview> SessionReviews { get; set; }



        public ApplicationContext() 
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MembershipOrder>()
                .HasOne(mo => mo.Client)
                .WithMany(u => u.Orders)
                .HasForeignKey(mo => mo.ClientId);

            modelBuilder.Entity<MembershipOrder>()
                .HasOne(mo => mo.Membership)
                .WithMany(m => m.Orders)
                .HasForeignKey(mo => mo.MembershipId);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Coach)
                .WithMany(c => c.Schedules)
                .HasForeignKey(s => s.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientSessionRecord>()
                .HasOne(s => s.MembershipOrder)
                .WithMany(c => c.ClientSession)
                .HasForeignKey(s => s.MembershipOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientSessionRecord>()
                .HasOne(s => s.Schedule)
                .WithMany(c => c.ClientSessionRecords)
                .HasForeignKey(s => s.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SporticsDB;Trusted_Connection=True;");
        }
    }
}
