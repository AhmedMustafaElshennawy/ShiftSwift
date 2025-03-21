
using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;
using ShiftSwift.Application.DTOs.member;

namespace ShiftSwift.Application.Features.accomplishment.Commands.AddAccomplishment
{
    public sealed record AddAccomplishmentCommand(string MemberId, string Title, string? Description, DateTime? DateAchieved)
        : IRequest<ErrorOr<ApiResponse<AccomplishmentResponse>>>;
}
