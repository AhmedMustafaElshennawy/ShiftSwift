using FluentValidation;

namespace ShiftSwift.Application.Features.rating.Commands.AddRating
{
    public class AddRatingCommandValidator : AbstractValidator<AddRatingCommand>
    {
        public AddRatingCommandValidator()
        {

            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("CompanyId is required.");

            RuleFor(x => x.RatedById)
                .NotEmpty().WithMessage("RatedById is required.");

            RuleFor(x => x.Score)
                .InclusiveBetween(1.0m, 5.0m)
                .WithMessage("Score must be between 1.0 and 5.0.");
        }
    }
}
