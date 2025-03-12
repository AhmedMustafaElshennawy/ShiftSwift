using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.job.Commands.DeletePostJob
{
    public sealed record DeletePostJobCommand(Guid JobId):IRequest<ErrorOr<ApiResponse<Deleted>>>;
}
