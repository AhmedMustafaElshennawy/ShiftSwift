
using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.accomplishment.Commands.AddAccomplishment
{
    public sealed record AddAccomplishmentCommand(string MemberId, string Title, string? Description, DateTime? DateAchieved)
        : IRequest<ErrorOr<ApiResponse<AccomplishmentResponse>>>;
}
