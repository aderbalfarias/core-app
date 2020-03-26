using CoreApp.Data.Mappings;
using CoreApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.Data.Config
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions<DemoContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SampleMap());
        }

        public DbSet<SampleEntity> Test { get; set; }
    }
}
