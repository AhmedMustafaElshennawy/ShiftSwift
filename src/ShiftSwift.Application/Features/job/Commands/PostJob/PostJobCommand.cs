using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.job.Commands.PostJob
{
    public sealed record PostJobCommand(string Title,
        string Description,
        string Location):IRequest<ErrorOr<ApiResponse<PostedJobResponse>>>;
}
