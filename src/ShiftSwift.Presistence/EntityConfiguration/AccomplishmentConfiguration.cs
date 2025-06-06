using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Presistence.EntityConfiguration
{
    public class AccomplishmentConfiguration : IEntityTypeConfiguration<Accomplishment>
    {
        public void Configure(EntityTypeBuilder<Accomplishment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Description)
                .HasMaxLength(500);
        }
    }
}
