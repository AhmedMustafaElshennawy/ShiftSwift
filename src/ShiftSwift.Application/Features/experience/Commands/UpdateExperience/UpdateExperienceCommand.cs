using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.experience.Commands.UpdateExperience;

public sealed record UpdateExperienceCommand(
    Guid ExperienceId,
    string MemberId,
    string Title,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description) : IRequest<ErrorOr<ApiResponse<UpdateExperienceResponse>>>;