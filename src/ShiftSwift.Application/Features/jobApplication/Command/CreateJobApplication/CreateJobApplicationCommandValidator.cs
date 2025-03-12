using FluentValidation;

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
        }
    }
}
