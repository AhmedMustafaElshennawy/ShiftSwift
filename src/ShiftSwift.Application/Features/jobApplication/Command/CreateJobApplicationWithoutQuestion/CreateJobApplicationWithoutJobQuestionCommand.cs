using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplicationWithoutQuestion;

public sealed record CreateJobApplicationWithoutJobQuestionCommand(
    Guid JobId,
    string MemberId) : IRequest<ErrorOr<ApiResponse<JobApplicationResponse>>>;


public sealed record JobApplicationWthoutQuestionResponse(Guid Id,
    Guid JobId,
    string MemberId,
    DateTime AppliedOn);