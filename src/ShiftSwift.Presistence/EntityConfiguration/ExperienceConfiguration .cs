using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.models.memberprofil;

namespace ShiftSwift.Presistence.EntityConfiguration
{
    public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
    {
        public void Configure(EntityTypeBuilder<Experience> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.StartDate)
                .IsRequired();

            builder.Property(e => e.EndDate)
                .IsRequired();
        }
    }
}
