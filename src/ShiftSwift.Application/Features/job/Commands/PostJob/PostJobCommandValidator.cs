using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftSwift.Application.Features.job.Commands.PostJob
{
    public sealed class PostJobCommandValidator:AbstractValidator<PostJobCommand>
    {
        public PostJobCommandValidator()
        {
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("Title is required.")
               .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");
        }
    }
}
