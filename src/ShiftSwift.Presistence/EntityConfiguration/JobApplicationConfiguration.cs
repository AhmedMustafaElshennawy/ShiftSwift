using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftSwift.Domain.shared;

namespace ShiftSwift.Presistence.Configurations
{
    public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
    {
        public void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            builder.ToTable("JobApplications");

            builder.HasKey(ja => ja.Id);

            builder.HasOne(ja => ja.Job)
                   .WithMany(j => j.JobApplications)
                   .HasForeignKey(ja => ja.JobId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ja => ja.Member)
                   .WithMany(m => m.JobApplications)
                   .HasForeignKey(ja => ja.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ja => ja.AppliedOn)
                   .IsRequired();

            builder.Property(ja => ja.Status)
                   .IsRequired();

            builder.Property(j => j.Location)
                .IsRequired(false);
        }
    }
}
