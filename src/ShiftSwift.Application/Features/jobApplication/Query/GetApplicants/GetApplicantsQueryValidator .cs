using FluentValidation;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetApplicants
{
    public sealed class GetApplicantsQueryValidator : AbstractValidator<GetApplicantsQuery>
    {
        public GetApplicantsQueryValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("JobId is required.")
                .NotEqual(Guid.Empty).WithMessage("JobId must be a valid GUID.");
        }
    }
}
