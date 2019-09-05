using CoreApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.Data.Mappings
{
    public class PrimaryContext : DbContext
    {
        public PrimaryContext(DbContextOptions<PrimaryContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TestMap());
        }

        public DbSet<TestEntity> Test { get; set; }
    }
}
