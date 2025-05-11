using FluentValidation;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetSpecificApplicant
{
    public sealed class GetSpecificApplicantQueryValidator : AbstractValidator<GetSpecificApplicantQuery>
    {
        public GetSpecificApplicantQueryValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("JobId is required.")
                .NotEqual(Guid.Empty).WithMessage("JobId must be a valid GUID.");


            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("MemberId is required.")
                .NotEqual(string.Empty).WithMessage("MemberId must be a valid GUID.");
        }
    }
}
