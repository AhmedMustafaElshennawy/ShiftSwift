using FluentValidation;

namespace ShiftSwift.Application.Features.skill.Queries.GetSkill
{
    public sealed class GetSkillQueryValidator : AbstractValidator<GetSkillQuery>
    {
        public GetSkillQueryValidator()
        {
            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("MemberId is required.");
        }
    }
}
