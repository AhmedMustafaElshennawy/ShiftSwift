using FluentValidation;

namespace ShiftSwift.Application.Features.education.Queries.GetEducation
{
    public sealed class GetEducationQueryValidator:AbstractValidator<GetEducationQuery>
    {
        public GetEducationQueryValidator()
        {
            RuleFor(X => X.MemberId).NotEmpty().WithMessage("MemberId Is required.");
        }
    }
}
