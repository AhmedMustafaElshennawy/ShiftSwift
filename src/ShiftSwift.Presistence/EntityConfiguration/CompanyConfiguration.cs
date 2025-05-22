using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.identity;

namespace ShiftSwift.Presistence.EntityConfiguration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(c => c.CompanyName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.Overview)
                .HasMaxLength(500).IsRequired(false);

            builder.Property(c => c.Field)
                .HasMaxLength(155).IsRequired(false);

            builder.Property(c => c.Country)
                .HasMaxLength(100).IsRequired(false); ;

            builder.Property(c => c.City)
                .HasMaxLength(100).IsRequired(false);

            builder.Property(c => c.Area)
                .HasMaxLength(100).IsRequired(false);

            builder.Property(c => c.DateOfEstablish)
                .IsRequired(false);
        }
    }
}