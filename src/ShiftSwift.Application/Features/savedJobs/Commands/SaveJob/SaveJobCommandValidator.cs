using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftSwift.Application.Features.savedJobs.Commands.SaveJob
{
    public sealed class SaveJobCommandValidator: AbstractValidator<SaveJobCommand>
    {
        public SaveJobCommandValidator() 
        {
            RuleFor(x => x.JobId)
           .NotEmpty().WithMessage("JobId is required.");

            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("MemberId is required.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("MemberId must be a valid GUID.");
        }
    }
}
