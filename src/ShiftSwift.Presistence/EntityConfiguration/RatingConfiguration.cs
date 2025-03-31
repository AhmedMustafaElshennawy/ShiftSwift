using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftSwift.Domain.shared;

namespace ShiftSwift.Presistence.EntityConfiguration
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Score)
                .IsRequired()
                .HasPrecision(2, 1)
                .HasDefaultValue(1.0);

            builder.Property(r => r.CreatedAt)
                .IsRequired();


            builder.HasOne(r => r.Company)
                .WithMany(c => c.Ratings)
                .HasForeignKey(r => r.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.RatedBy)
                .WithMany()
                .HasForeignKey(r => r.RatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
