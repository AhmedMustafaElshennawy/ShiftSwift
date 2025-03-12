using FluentValidation;

namespace ShiftSwift.Application.Features.job.Commands.UpdatePostJob
{
    public sealed class UpdatePostJobCommandValidator:AbstractValidator<UpdatePostJobCommand>
    {
        public UpdatePostJobCommandValidator()
        {
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("Title is required.")
               .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");

            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("Job ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Job ID must be a valid GUID.");
        }
    }
}
