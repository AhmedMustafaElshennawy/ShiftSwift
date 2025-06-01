using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Commands.RemoveFromShortlist
{
    public sealed record RemoveFromShortlistCommand(
        Guid JobId,
        string MemberId
    ) : IRequest<ErrorOr<ApiResponse<string>>>;
}
