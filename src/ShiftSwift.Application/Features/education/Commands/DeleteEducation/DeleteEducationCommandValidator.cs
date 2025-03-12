using FluentValidation;

namespace ShiftSwift.Application.Features.education.Commands.DeleteEducation
{
    public sealed class DeleteEducationCommandValidator:AbstractValidator<DeleteEducationCommand>
    {
        public DeleteEducationCommandValidator()
        {
            RuleFor(X=>X.MemberId).NotEmpty().WithMessage("MemberId Is required.");

        }
    }
}
