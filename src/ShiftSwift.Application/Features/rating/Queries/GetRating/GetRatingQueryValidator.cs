using FluentValidation;

namespace ShiftSwift.Application.Features.rating.Queries.GetRating
{
    public sealed class GetRatingQueryValidator : AbstractValidator<GetRatingQuery>
    {
        public GetRatingQueryValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required.");
        }
    }
}
