using FluentValidation;
namespace ShiftSwift.Application.Features.searchJobs.Queries.SearchJobs
{
    public sealed class SearchJobsQueryValidator : AbstractValidator<SearchJobsQuery>
    {
        public SearchJobsQueryValidator()
        {
            RuleFor(x => x.JobTypeIdFilterValue)
               .Must(BeAValidJobType)
               .WithMessage("Invalid JobTypeIdFilterValue. Allowed values: 0(none), 1(FullTime), 2(PartTime), 3(Freelance).");

            RuleFor(x => x.MinSalary)
                .GreaterThanOrEqualTo(0).When(x => x.MinSalary.HasValue)
                .WithMessage("MinSalary must be >= 0.");

            RuleFor(x => x.MaxSalary)
                .GreaterThanOrEqualTo(0).When(x => x.MaxSalary.HasValue)
                .WithMessage("MaxSalary must be >= 0.");

            RuleFor(x => x)
                .Must(x => !x.MinSalary.HasValue || !x.MaxSalary.HasValue || x.MinSalary <= x.MaxSalary)
                .WithMessage("MinSalary must be less than or equal to MaxSalary.");

            RuleFor(x => x.SortBy)
               .Must(sortBy => sortBy == null ||
                              sortBy.ToLower() == "latest" ||
                              sortBy.ToLower() == "oldest")
               .WithMessage("SortBy must be either 'latest' or 'oldest'");
        }

        private bool BeAValidJobType(int jobTypeId) =>
            jobTypeId is 0 or 1 or 2 or 3;

    }
}
