using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.skill.Commands.AddSkill;

public sealed record AddSkillCommand(string MemberId, string Name)
    : IRequest<ErrorOr<ApiResponse<SkillResponse>>>;