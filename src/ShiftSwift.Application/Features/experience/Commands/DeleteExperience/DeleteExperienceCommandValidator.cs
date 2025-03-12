using FluentValidation;
using ShiftSwift.Application.Features.experience.Commands.DeleteEducation;

namespace ShiftSwift.Application.Features.experience.Commands.DeleteExperience
{
    public sealed class DeleteExperienceCommandValidator:AbstractValidator<DeleteExperienceCommand>
    {
        public DeleteExperienceCommandValidator()
        {
            RuleFor(X => X.MemberId).NotEmpty().WithMessage("MemberId Is required.");
        }
    }
}
