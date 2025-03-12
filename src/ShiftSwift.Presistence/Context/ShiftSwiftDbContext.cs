using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.memberprofil;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Domain.shared;


namespace ShiftSwift.Presistence.Context
{
    public class ShiftSwiftDbContext : IdentityDbContext<Account, IdentityRole, string>
    {
        

        public ShiftSwiftDbContext (DbContextOptions<ShiftSwiftDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShiftSwiftDbContext).Assembly);
        }
        public DbSet<Member> Members { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Accomplishment> Accomplishments { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}