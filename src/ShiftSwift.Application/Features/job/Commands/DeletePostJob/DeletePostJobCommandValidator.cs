using FluentValidation;

namespace ShiftSwift.Application.Features.job.Commands.DeletePostJob;

public sealed class DeletePostJobCommandValidator:AbstractValidator<DeletePostJobCommand>
{
    public DeletePostJobCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty().WithMessage("Job ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Job ID must be a valid GUID.");
    }
}