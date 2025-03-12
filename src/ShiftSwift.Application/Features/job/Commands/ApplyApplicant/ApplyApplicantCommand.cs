using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.job.Commands.ApplyApplicant
{
    public sealed record ApplyApplicantCommand(Guid JobId, string MemberId)
        : IRequest<ErrorOr<ApiResponse<ApplyApplicantResponse>>>;
}
