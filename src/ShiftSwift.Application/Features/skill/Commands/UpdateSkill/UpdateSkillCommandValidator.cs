using FluentValidation;

namespace ShiftSwift.Application.Features.skill.Commands.UpdateSkill;

public sealed class UpdateSkillCommandValidator : AbstractValidator<UpdateSkillCommand>
{
    public UpdateSkillCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("MemberId is required.")
            .MaximumLength(450).WithMessage("MemberId cannot exceed 450 characters.");

        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Skill name is required.")
            .MaximumLength(100).WithMessage("Skill name cannot exceed 100 characters.");
    }
}