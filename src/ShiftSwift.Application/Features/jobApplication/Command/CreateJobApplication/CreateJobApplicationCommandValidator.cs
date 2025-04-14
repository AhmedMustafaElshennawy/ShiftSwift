using FluentValidation;
using ShiftSwift.Domain.Enums;

namespace ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplication
{
    public class CreateJobApplicationCommandValidator : AbstractValidator<CreateJobApplicationCommand>
    {
        public CreateJobApplicationCommandValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("JobId is required.")
                .NotEqual(Guid.Empty).WithMessage("JobId must be a valid GUID.");

            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("MemberId is required.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("MemberId must be a valid GUID.");

            RuleFor(x => x.ApplicationStatus)
                .NotEmpty().WithMessage("Job type is required.")
                .Must(g => g == 1 || g == 2 || g == 3)
                .WithMessage("Invalid Application Status . Allowed values: 1 (Pending), 2 (Accepted), 3 (Rejected).");
        }
    }
}
