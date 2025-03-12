using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Presistence.EntityConfiguration
{
    public class SavedJobConfiguration : IEntityTypeConfiguration<SavedJob>
    {
        public void Configure(EntityTypeBuilder<SavedJob> builder)
        {

            builder.Property(sj => sj.SavedOn)
                .IsRequired();
        }
    }

}
