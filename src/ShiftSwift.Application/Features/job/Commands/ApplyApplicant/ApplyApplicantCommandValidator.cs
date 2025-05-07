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

            RuleFor(x => x.ApplicationStatus)
                 .NotEmpty().WithMessage("Job type is required.")
                 .Must(g => g == 1 || g == 2 || g == 3 || g == 4 || g == 5)
                 .WithMessage("Invalid Application Status . Allowed values: 1 (Pending), 2 (Accepted), 3 (Rejected), 4 (Shortlisted), 5 (RemovedFromShortlist).");
        }
    }
}
