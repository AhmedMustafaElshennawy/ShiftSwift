using FluentValidation;

namespace ShiftSwift.Application.Features.accomplishment.Commands.DeleteAccomplishment
{
    public class DeleteAccomplishmentCommandValidator : AbstractValidator<DeleteAccomplishmentCommand>
    {
        public DeleteAccomplishmentCommandValidator()
        {
            RuleFor(command => command.MemberId)
                .NotEmpty().WithMessage("Member ID is required.");
        }
    }
}
