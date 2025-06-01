using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.savedJobs.Commands.SaveJob
{
    public sealed record SaveJobCommand(Guid JobId,
        string MemberId):IRequest<ErrorOr<ApiResponse<bool>>>;
}
