using FluentValidation;

namespace ShiftSwift.Application.Features.Authentication.Queries.LogInMember
{
    public sealed class LoginMemberQueryValidator : AbstractValidator<LoginMemberQuery>
    {
        public LoginMemberQueryValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
