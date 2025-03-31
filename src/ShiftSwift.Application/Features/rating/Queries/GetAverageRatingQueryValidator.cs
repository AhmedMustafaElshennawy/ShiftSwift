using FluentValidation;

namespace ShiftSwift.Application.Features.rating.Queries.GetRating
{
    public sealed class GetAverageRatingQueryValidator : AbstractValidator<GetAverageRatingQuery>
    {
        public GetAverageRatingQueryValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required.");
        }
    }
}
