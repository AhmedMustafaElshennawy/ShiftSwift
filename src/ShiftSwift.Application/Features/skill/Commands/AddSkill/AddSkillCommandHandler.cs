using ErrorOr;
using MediatR;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Application.Features.skill.Commands.AddSkill;

public sealed class AddSkillCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<AddSkillCommand, ErrorOr<ApiResponse<SkillResponse>>>
{
    public async Task<ErrorOr<ApiResponse<SkillResponse>>> Handle(
        AddSkillCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Authentication & Authorization
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
                description: "Access denied. Only members can add skills.");
        }

        if (currentUser.UserId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "Access denied. Invalid MemberId.");
        }

        var newSkill = new Skill
        {
            Id = Guid.NewGuid(),
            MemberId = currentUser.UserId,
            Name = request.Name
        };

        await unitOfWork.Skills.AddEntityAsync(newSkill);
        await unitOfWork.CompleteAsync(cancellationToken);

        var response = new SkillResponse(
            currentUser.UserId,
            request.Name);

        return new ApiResponse<SkillResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "Skill added successfully.",
            Data = response
        };
    }
}