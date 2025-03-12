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

            builder.Property(c => c.Description)
                   .HasMaxLength(500);
        }
    }
}
