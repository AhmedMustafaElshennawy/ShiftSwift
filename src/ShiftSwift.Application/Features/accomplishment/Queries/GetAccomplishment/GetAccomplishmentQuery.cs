using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.accomplishment.Queries.GetAccomplishment
{
    public sealed record GetAccomplishmentQuery(string MemberId)
        : IRequest<ErrorOr<ApiResponse<AccomplishmentResponse>>>;
}
