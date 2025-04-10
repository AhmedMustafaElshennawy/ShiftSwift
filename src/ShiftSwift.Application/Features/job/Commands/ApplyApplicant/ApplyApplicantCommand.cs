using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.Enums;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.job.Commands.ApplyApplicant
{
    public sealed record ApplyApplicantCommand(
        Guid JobId,
        string MemberId,
        ApplicationStatus ApplicationStatus): IRequest<ErrorOr<ApiResponse<ApplyApplicantResponse>>>;
}
