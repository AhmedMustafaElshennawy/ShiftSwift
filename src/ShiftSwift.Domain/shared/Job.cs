using ShiftSwift.Domain.Enums;
using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Domain.shared
{
    public class Job
    {
        public Guid Id { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkModeEnum WorkMode { get; set; }
        public string Location { get; set; }
        public JobTypeEnum JobType { get; set; }
        public decimal? Salary { get; set; }
        public SalaryTypeEnum? SalaryType { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.UtcNow;
        public string Requirements { get; set; }
        public string Keywords { get; set; }
        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
        public ICollection<SavedJob> SavedJobs { get; set; } = new HashSet<SavedJob>();
    }
}
