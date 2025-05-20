using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.job.Commands.UpdatePostJob
{
    public sealed record UpdatePostJobCommand(Guid JobId,
        string Title,
        string Description,
        string Location,
        int JobType,
        int WorkMode,
        decimal Salary,
        int SalaryType,
        string Requirements,
        string Keywords,
       List<JobQuestionDTO> Questions) : IRequest<ErrorOr<ApiResponse<PostedJobResponse>>>;
}
