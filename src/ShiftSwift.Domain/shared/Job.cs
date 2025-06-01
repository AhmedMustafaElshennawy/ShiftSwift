using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.memberprofil;
using ShiftSwift.Domain.Shared;

namespace ShiftSwift.Domain.shared;

public class Job
{
    public Guid Id { get; set; }

    public required string CompanyId { get; set; }
    public Company Company { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int WorkModeId { get; set; }
    public  required string Location { get; set; }
    public int JobTypeId { get; set; }
    public required decimal Salary { get; set; }
    public int SalaryTypeId { get; set; }
    public DateTime PostedOn { get; set; } = DateTime.UtcNow;
    public required string Requirements { get; set; }
    public required string Keywords { get; set; }
    public ICollection<JobQuestion> Questions { get; set; } = new List<JobQuestion>();
    public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
    public ICollection<SavedJob> SavedJobs { get; set; } = new HashSet<SavedJob>();
}