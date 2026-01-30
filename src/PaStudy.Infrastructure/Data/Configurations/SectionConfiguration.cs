using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaStudy.Core.Entities;

namespace PaStudy.Infrastructure.Data.Configurations;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description);

        builder.Property(s => s.Order)
            .IsRequired();

        builder.HasMany(s => s.Assignments)
            .WithOne(a => a.Section)
            .HasForeignKey(a => a.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Course).WithMany(c => c.Sections)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
