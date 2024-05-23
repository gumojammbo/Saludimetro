using Saludimetro.Models;
using Saludimetro.Utilities;
using Microsoft.EntityFrameworkCore;


namespace Saludimetro.DataAccess
{
	public class PatientDbContext : DbContext
	{
		public DbSet<Patient> Patients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbConnection = $"Filename={DBConnection.ReturnPath("patients.db")}";
            optionsBuilder.UseSqlite(dbConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(col => col.PatientID);
                entity.Property(col => col.PatientID).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}

