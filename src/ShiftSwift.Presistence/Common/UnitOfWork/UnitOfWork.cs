using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.memberprofil;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Domain.shared;
using ShiftSwift.Domain.Shared;
using ShiftSwift.Presistence.Common.Repository;
using ShiftSwift.Presistence.Context;

namespace ShiftSwift.Presistence.Common.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ShiftSwiftDbContext _shiftSwiftDbContext;
        public IBaseRepository<Company> Companies { get; set; }
        public IBaseRepository<Member> Members { get; set; }
        public IBaseRepository<Accomplishment> Accomplishments { get; set; }
        public IBaseRepository<Education> Educations { get; set; }
        public IBaseRepository<Experience> Experiences { get; set; }
        public IBaseRepository<SavedJob> SavedJobs { get; set; }
        public IBaseRepository<Skill> Skills { get; set; }
        public IBaseRepository<Job> Jobs { get; set; }
        public IBaseRepository<JobApplication> JobApplications { get; set; }
        public IBaseRepository<Rating> Ratings { get; set; }
        public IBaseRepository<JobQuestion> JobQuestions { get; set; }
        public IBaseRepository<ApplicationAnswer> ApplicationAnswers { get; set; }
        public UnitOfWork(ShiftSwiftDbContext shiftSwiftDbContext)
        {
            Companies = new BaseRepository<Company>(shiftSwiftDbContext);
            Members = new BaseRepository<Member>(shiftSwiftDbContext);
            Accomplishments = new BaseRepository<Accomplishment>(shiftSwiftDbContext);
            Educations = new BaseRepository<Education>(shiftSwiftDbContext);
            Experiences = new BaseRepository<Experience>(shiftSwiftDbContext);
            SavedJobs = new BaseRepository<SavedJob>(shiftSwiftDbContext);
            Skills = new BaseRepository<Skill>(shiftSwiftDbContext);
            Jobs = new BaseRepository<Job>(shiftSwiftDbContext);
            JobApplications = new BaseRepository<JobApplication>(shiftSwiftDbContext);
            Ratings = new BaseRepository<Rating>(shiftSwiftDbContext);
            JobQuestions = new BaseRepository<JobQuestion>(shiftSwiftDbContext);
            ApplicationAnswers = new BaseRepository<ApplicationAnswer>(shiftSwiftDbContext);
            _shiftSwiftDbContext = shiftSwiftDbContext;
        }

        public int Complete() => _shiftSwiftDbContext.SaveChanges();
        public async Task<int> CompleteAsync(CancellationToken cancellationToken) => await _shiftSwiftDbContext.SaveChangesAsync(cancellationToken);
        public void Dispose() => _shiftSwiftDbContext?.Dispose();
    }
}
