using MedRoute.Models;
using Microsoft.EntityFrameworkCore;

namespace MedRoute.Database
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        #region DbSet
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<ServeStatus> ServeStatuses { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<MedicalRecord>()
                .HasOne(e => e.Patient)
                .WithMany(e => e.PatientRecords);

            modelBuilder
                .Entity<MedicalRecord>()
                .HasOne(e => e.ServeUser)
                .WithMany(e => e.ServeRecords);

            base.OnModelCreating(modelBuilder);
        }
    }
}
