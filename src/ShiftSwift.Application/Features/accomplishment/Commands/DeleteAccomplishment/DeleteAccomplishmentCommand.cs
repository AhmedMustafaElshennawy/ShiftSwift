using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.accomplishment.Commands.DeleteAccomplishment
{
    public sealed record DeleteAccomplishmentCommand(string MemberId)
        : IRequest<ErrorOr<ApiResponse<Deleted>>>;
}
