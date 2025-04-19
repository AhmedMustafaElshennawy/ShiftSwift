using ShiftSwift.Domain.memberprofil;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Domain.shared;

namespace ShiftSwift.Domain.identity
{
    public class Member : Account
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Experience> Experiences { get; set; } = new HashSet<Experience>();
        public ICollection<Education> Educations { get; set; } = new HashSet<Education>();
        public ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();
        public ICollection<Accomplishment> Accomplishments { get; set; } = new HashSet<Accomplishment>();
        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
        public ICollection<SavedJob> SavedJobs { get; set; } = new HashSet<SavedJob>();
        public int ProfileViews { get; set; }
        public int GenderId { get; set; }
        public string Location { get; set; }
        public DateTime BirthDate { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}