using FluentValidation;

namespace ShiftSwift.Application.Features.experience.Queries.GetExperience;

public sealed class GetExperienceQueryValidator : AbstractValidator<GetExperienceQuery>
{
    public GetExperienceQueryValidator()
    {
        RuleFor(X => X.MemberId).NotEmpty().WithMessage("MemberId Is required.");
    }
}