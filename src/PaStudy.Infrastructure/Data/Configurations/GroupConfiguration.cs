using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;

namespace PaStudy.Infrastructure.Data.Configurations;
public class GroupConfiguration: IEntityTypeConfiguration<Group>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(g => g.Id);

        builder
            .HasIndex(g => new { g.InstitutionNumber, g.GroupNumber })
            .IsUnique();

        builder
            .Property(g => g.GroupNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder
            .Property(g => g.InstitutionNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasMany(g => g.Students)
            .WithOne(s => s.Group)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.CuratorOfGroup)
            .WithOne(b => b.GroupOfCurator)
            .HasForeignKey<Group>(g => g.CuratorOfGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

