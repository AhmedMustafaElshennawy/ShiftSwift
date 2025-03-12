using FluentValidation;

namespace ShiftSwift.Application.Features.Authentication.Queries.LogInCompany
{
    public sealed class LoginCompanyQueryValidator : AbstractValidator<LoginCompanyQuery>
    {
        public LoginCompanyQueryValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
