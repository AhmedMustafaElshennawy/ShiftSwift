using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.models.memberprofil;

namespace ShiftSwift.Presistence.EntityConfiguration
{
    public class EducationConfiguration : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FieldOfStudy)
                .IsRequired()
                .HasMaxLength(100); 

            builder.Property(e => e.LevelOfEducation)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.SchoolName)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
