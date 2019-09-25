using efcore_cosmosdb.Models;
using Microsoft.EntityFrameworkCore;

namespace efcore_cosmosdb
{
    public class JobContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
               "YOUR-COSMOS-URI",
                "YOUR-COSMOS-KEY",
                "EFCoreTest"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Jobs");

            modelBuilder.Entity<Job>().OwnsOne(j => j.Address);
            modelBuilder.Entity<Job>().OwnsMany(j => j.Contacts);
            modelBuilder.Entity<Job>().HasOne(j => j.AssignedResource);
        }
    }
}