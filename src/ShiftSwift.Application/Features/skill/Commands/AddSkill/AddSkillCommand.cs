using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.skill.Commands.AddSkill
{
    public sealed record AddSkillCommand(string MemberId, string Name)
        : IRequest<ErrorOr<ApiResponse<SkillResponse>>>;
}

