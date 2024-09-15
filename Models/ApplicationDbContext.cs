using Microsoft.EntityFrameworkCore;
using BeautySaloon.Models;

namespace BeautySaloon.Models
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Services> Services { get; set; }

        public DbSet<Service> Service { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Master> Master { get; set; }
        public DbSet<MasterService> MasterService { get; set; }

        public DbSet<WorkSchedule> WorkSchedules { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MasterService>()
                .HasOne(s => s.Appointment)
                .WithOne(a => a.MasterService)
                .HasForeignKey<Appointment>(a => a.MasterServiceId);
           
            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.UserId);

        
        }
    }
}
