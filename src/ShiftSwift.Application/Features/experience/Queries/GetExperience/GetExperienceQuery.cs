using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.experience.Queries.GetExperience;

public sealed record GetExperienceQuery(string MemberId)
    : IRequest<ErrorOr<ApiResponse<List<ExperienceResponse>>>>;