using FluentValidation;

namespace ShiftSwift.Application.Features.skill.Commands.AddSkill
{
    public class AddSkillCommandValidator : AbstractValidator<AddSkillCommand>
    {
        public AddSkillCommandValidator()
        {
            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("MemberId is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Skill name is required.")
                .MaximumLength(120).WithMessage("Skill name must not exceed 120 characters.");
        }
    }
}
