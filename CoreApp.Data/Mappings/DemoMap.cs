using CoreApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApp.Data.Mappings;

public class DemoMap : IEntityTypeConfiguration<DemoEntity>
{
    public void Configure(EntityTypeBuilder<DemoEntity> builder)
    {
        builder.ToTable("Demo", "dbo");

        builder.HasKey(k => k.Id);

        builder.Property(p => p.Text)
            .HasMaxLength(2000);

        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.Presenter)
            .HasMaxLength(60)
            .IsRequired();
    }
}