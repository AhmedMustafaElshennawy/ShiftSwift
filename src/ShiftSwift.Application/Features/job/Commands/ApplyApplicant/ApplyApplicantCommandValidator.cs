using FluentValidation;

namespace ShiftSwift.Application.Features.job.Commands.ApplyApplicant
{
    public sealed class ApplyApplicantCommandValidator: AbstractValidator<ApplyApplicantCommand>
    {
        public ApplyApplicantCommandValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("JobId is required.");

            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("MemberId is required.")
                .MaximumLength(100).WithMessage("MemberId cannot exceed 100 characters.");

        }
    }
}
