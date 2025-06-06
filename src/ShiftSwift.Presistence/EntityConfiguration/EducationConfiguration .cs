using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Presistence.EntityConfiguration;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Level)
            .IsRequired()
            .HasMaxLength(100); 

        builder.Property(e => e.Faculty)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.UniversityName)
            .IsRequired()
            .HasMaxLength(150);
    }
}