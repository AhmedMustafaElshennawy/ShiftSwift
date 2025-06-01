using FluentValidation;

namespace ShiftSwift.Application.Features.experience.Commands.DeleteExperience;

public sealed class DeleteExperienceCommandValidator:AbstractValidator<DeleteExperienceCommand>
{
    public DeleteExperienceCommandValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty().WithMessage("MemberId Is required.");
    }
}