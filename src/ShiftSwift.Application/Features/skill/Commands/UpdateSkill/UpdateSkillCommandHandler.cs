using ErrorOr;
using MediatR;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.skill.Commands.UpdateSkill;

public sealed class UpdateSkillCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Skill> skillRepository)
    : IRequestHandler<UpdateSkillCommand, ErrorOr<ApiResponse<SkillResponse>>>
{
    public async Task<ErrorOr<ApiResponse<SkillResponse>>> Handle(
        UpdateSkillCommand request,
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
                description: "Access denied. Only members can update skills.");
        }

        if (currentUser.UserId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "Access denied. Invalid MemberId.");
        }

        var existingSkill = await skillRepository.Entites()
            .FirstOrDefaultAsync(s =>
                    s.Id == request.SkillId &&
                    s.MemberId == currentUser.UserId,
                cancellationToken);

        if (existingSkill == null)
        {
            return Error.NotFound(
                code: "Skill.NotFound",
                description: "Skill not found or you don't have permission to update it.");
        }

        existingSkill.Name = request.Name;
        await unitOfWork.Skills.UpdateAsync(existingSkill);
        await unitOfWork.CompleteAsync(cancellationToken);

        var response = new SkillResponse(
            currentUser.UserId,
            request.Name);

        return new ApiResponse<SkillResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Skill updated successfully.",
            Data = response
        };
    }
}