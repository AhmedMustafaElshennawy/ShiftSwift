using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.skill.Commands.DeleteSkill;

public sealed class DeleteSkillCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Skill> skillRepository)
    : IRequestHandler<DeleteSkillCommand, ErrorOr<ApiResponse<Deleted>>>
{
    public async Task<ErrorOr<ApiResponse<Deleted>>> Handle(DeleteSkillCommand request,
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
                description: "Access denied. Only members can delete skills."
            );
        }

        if (currentUser.UserId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}"
            );
        }

        var skill = await skillRepository.Entites()
            .FirstOrDefaultAsync(x =>
                    x.MemberId == request.MemberId &&
                    x.Id == request.SkillId,
                cancellationToken);

        if (skill is null)
        {
            return Error.NotFound(
                code: "Skill.NotFound",
                description: "Skill not found for the specified ID and user.");
        }

        var deletionResult = await unitOfWork.Skills.DeleteAsync(skill);
        if (!deletionResult)
        {
            return Error.Failure(
                code: "Skill.Failure",
                description: "Failed to delete skill.");
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        return new ApiResponse<Deleted>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.NoContent,
            Message = "Skill deleted successfully.",
            Data = new Deleted() // Use proper Deleted marker object
        };
    }
}