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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SporticsDB;Trusted_Connection=True;");
        }
    }
}
