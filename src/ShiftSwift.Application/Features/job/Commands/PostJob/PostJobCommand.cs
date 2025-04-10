using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Domain.Enums;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.job.Commands.PostJob
{
    public sealed record PostJobCommand(string Title,
        string Description,
        string Location,
        JobTypeEnum JobType):IRequest<ErrorOr<ApiResponse<PostedJobResponse>>>;
}
