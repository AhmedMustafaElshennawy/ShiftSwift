using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.skill.Queries.GetSkill;

public sealed class GetSkillQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Skill> skillRepository)
    : IRequestHandler<GetSkillQuery, ErrorOr<ApiResponse<List<SkillResponse>>>>
{
    public async Task<ErrorOr<ApiResponse<List<SkillResponse>>>> Handle(
        GetSkillQuery request,
        CancellationToken cancellationToken)
    {
        var currentUserResult = await currentUserProvider.GetCurrentUser();
        if (currentUserResult.IsError)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: currentUserResult.Errors.FirstOrDefault().Description ?? "User is not authenticated.");
        }

        var currentUser = currentUserResult.Value;
        if (!currentUser.Roles.Contains("Member"))
        {
            return Error.Forbidden(
                code: "User.Forbidden",
                description: "Access denied. Only members can view skills.");
        }

        if (request.MemberId != currentUser.UserId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "Access denied. Invalid MemberId.");
        }

        var skills = await skillRepository.Entites()
            .Where(x => x.MemberId == currentUser.UserId)
            .ToListAsync(cancellationToken);

        var skillResponses = skills.Select(skill => new SkillResponse(
            currentUser.UserId,
            skill.Name
        )).ToList();

        return new ApiResponse<List<SkillResponse>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = skills.Count > 0
                ? "Skills retrieved successfully."
                : "No skills found for this member.",
            Data = skillResponses
        };
    }
}