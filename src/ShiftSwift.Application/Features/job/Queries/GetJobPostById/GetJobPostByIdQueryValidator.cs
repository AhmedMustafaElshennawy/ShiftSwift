using FluentValidation;

namespace ShiftSwift.Application.Features.job.Queries.GetJobPostById
{
    public sealed class GetJobPostByIdQueryValidator : AbstractValidator<GetJobPostByIdQuery>
    {
        public GetJobPostByIdQueryValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("JobId is required.");
        }
    }
}
