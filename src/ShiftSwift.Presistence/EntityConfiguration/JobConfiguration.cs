﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.shared;

namespace ShiftSwift.Presistence.EntityConfiguration
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(j => j.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(j => j.Location)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(j => j.PostedOn)
                .IsRequired();

            builder.HasOne(j => j.Company)
                   .WithMany(c => c.Jobs)
                   .HasForeignKey(j => j.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(j => j.JobApplications)
                           .WithOne(ja => ja.Job)
                           .HasForeignKey(ja => ja.JobId)
                           .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(j => j.SavedJobs)
                   .WithOne(sj => sj.Job)
                   .HasForeignKey(sj => sj.JobId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}