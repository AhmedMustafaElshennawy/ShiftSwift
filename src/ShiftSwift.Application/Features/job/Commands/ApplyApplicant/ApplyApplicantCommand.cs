using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Commands.ApplyApplicant;

public sealed record ApplyApplicantCommand(
    Guid JobId,
    string MemberId,
    int ApplicationStatus): IRequest<ErrorOr<ApiResponse<ApplyApplicantResponse>>>;