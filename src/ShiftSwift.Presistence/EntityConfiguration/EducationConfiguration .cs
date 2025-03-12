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

            builder.Property(e => e.Degree)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Institution)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
