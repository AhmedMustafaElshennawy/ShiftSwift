using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.identity;


namespace ShiftSwift.Presistence.EntityConfiguration
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(m => m.FirstName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(m => m.GenderId)
                   .HasConversion<int>();

            builder.Property(m => m.MiddleName)
                   .HasMaxLength(10);

            builder.Property(m => m.LastName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(m => m.ProfileViews)
                   .HasDefaultValue(0);
        }
    }
}