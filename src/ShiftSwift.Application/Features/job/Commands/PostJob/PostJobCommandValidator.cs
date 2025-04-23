using FluentValidation;
using ShiftSwift.Domain.Enums;
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

            RuleFor(x => x.JobType)
               .NotEmpty().WithMessage("Job type is required.")
               .Must(g => g == 1 || g == 2 || g == 3)
               .WithMessage("Invalid job Type . Allowed values: 1 (FullTime), 2 (PartTime), 3 (Freelance).");

            RuleFor(x => x.WorkMode)
               .NotEmpty().WithMessage("work mode is required.")
               .Must(g => g == 1 || g == 2 || g == 3)
               .WithMessage("Invalidwork mode . Allowed values: 1 (OnSite), 2 (Remotely), 3 (Hybrid).");

            RuleFor(x => x.SalaryType)
                 .NotEmpty().WithMessage("Salary Type is required.")
                 .Must(g => g == 1 || g == 2 || g == 3)
                 .WithMessage("Salary Type . Allowed values: 1 (PerMonth), 2 (PerHour), 3 (Contract).");

            RuleFor(x => x.Salary)
                .GreaterThanOrEqualTo(0).WithMessage("Salary must be a positive value.");

            RuleFor(x => x.Requirements)
                .NotEmpty().WithMessage("Requirements are required.");

            RuleFor(x => x.Keywords)
                .NotEmpty().WithMessage("Keywords are required.");

        }
    }
}
