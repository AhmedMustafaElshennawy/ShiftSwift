using FluentValidation;

namespace ShiftSwift.Application.Features.accomplishment.Commands.AddAccomplishment
{
    public class AddAccomplishmentCommandValidator : AbstractValidator<AddAccomplishmentCommand>
    {
        public AddAccomplishmentCommandValidator()
        {
            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("MemberId is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(120).WithMessage("Title must not exceed 120 characters.");
        }
    }
}

