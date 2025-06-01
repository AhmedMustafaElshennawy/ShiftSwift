using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.Enums;

namespace ShiftSwift.Application.Features.job.Commands.PostJob
{
    public sealed record PostJobCommand(string Title,
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
