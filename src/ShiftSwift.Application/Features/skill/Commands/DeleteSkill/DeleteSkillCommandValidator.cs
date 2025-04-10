using FluentValidation;

namespace ShiftSwift.Application.Features.skill.Commands.DeleteSkill
{
    public class DeleteSkillCommandValidator : AbstractValidator<DeleteSkillCommand>
    {
        public DeleteSkillCommandValidator()
        {

            RuleFor(command => command.MemberId)
                .NotEmpty().WithMessage("Member ID is required.");
        }
    }
}
