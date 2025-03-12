using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftSwift.Domain.identity;

namespace ShiftSwift.Presistence.EntityConfiguration
{
    namespace ShiftSwift.Presistence.EntityConfiguration
    {
        public class AccountConfiguration : IEntityTypeConfiguration<Account>
        {
            public void Configure(EntityTypeBuilder<Account> builder)
            {
                builder.ToTable("Accounts"); 

                builder.HasDiscriminator<string>("Discriminator")
                .HasValue<Company>("Company")
                .HasValue<Member>("Member");

                builder.Property("Discriminator")
                       .HasMaxLength(50)
                       .IsRequired();
            }
        }
    }
}