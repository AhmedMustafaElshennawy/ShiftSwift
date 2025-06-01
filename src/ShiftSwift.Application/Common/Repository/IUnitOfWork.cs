using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.memberprofil;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Domain.shared;
using ShiftSwift.Domain.Shared;


namespace ShiftSwift.Application.Common.Repository;

public interface IUnitOfWork : IDisposable
{
    public IBaseRepository<Company> Companies { get; }
    public IBaseRepository<Member> Members { get; }
    public IBaseRepository<Accomplishment> Accomplishments { get; }
    public IBaseRepository<Education> Educations { get; }
    public IBaseRepository<Experience> Experiences { get; }
    public IBaseRepository<SavedJob> SavedJobs { get; }
    public IBaseRepository<Skill> Skills { get; }
    public IBaseRepository<Job> Jobs { get; }
    public IBaseRepository<JobApplication> JobApplications { get; }
    public IBaseRepository<Rating> Ratings { get; }
    public IBaseRepository<JobQuestion> JobQuestions { get; }
    public IBaseRepository<ApplicationAnswer> ApplicationAnswers { get; }

    Task<int> CompleteAsync(CancellationToken cancellationToken);
    int Complete();
}