using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.savedJobs.Commands.SaveJob
{
    public sealed record SaveJobCommand(Guid JobId,
        string MemberId):IRequest<ErrorOr<ApiResponse<bool>>>;
}
