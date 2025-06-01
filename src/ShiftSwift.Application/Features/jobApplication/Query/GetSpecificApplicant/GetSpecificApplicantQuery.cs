using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetSpecificApplicant
{
    public sealed record GetSpecificApplicantQuery(Guid JobId, string MemberId)
        : IRequest<ErrorOr<ApiResponse<SpecificApplicantResponse>>>;
}
