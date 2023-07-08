using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Diagnoses)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "PatientDiagnose",
                    j => j.HasOne<Diagnose>().WithMany().HasForeignKey("DiagnoseId"),
                    j => j.HasOne<Patient>().WithMany().HasForeignKey("PatientId")
                );
        }

    }
}
