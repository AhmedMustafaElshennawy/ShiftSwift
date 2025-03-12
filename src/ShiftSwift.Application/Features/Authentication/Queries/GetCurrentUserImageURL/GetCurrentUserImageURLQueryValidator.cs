using FluentValidation;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserImageURL
{
    public sealed class GetCurrentUserImageURLQueryValidator : AbstractValidator<GetCurrentUserImageURLQuery>
    {
        public GetCurrentUserImageURLQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
