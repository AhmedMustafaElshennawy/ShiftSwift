using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftSwift.Domain.Shared;

namespace ShiftSwift.Presistence.Configurations
{
    public class JobQuestionConfiguration : IEntityTypeConfiguration<JobQuestion>
    {
        public void Configure(EntityTypeBuilder<JobQuestion> builder)
        {
            builder.ToTable("JobQuestions");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.QuestionText)
                   .IsRequired()
                   .HasMaxLength(400);

            builder.Property(q => q.QuestionType)
                   .IsRequired();

            builder.HasOne(q => q.Job)
                   .WithMany(j => j.Questions)
                   .HasForeignKey(q => q.JobId)
                   .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
