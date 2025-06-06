using System.Net;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Application.Features.experience.Commands.DeleteExperience;

public sealed class DeleteExperienceCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Experience> experienceRepository)
    : IRequestHandler<DeleteExperienceCommand, ErrorOr<ApiResponse<Deleted>>>
{
    public async Task<ErrorOr<ApiResponse<Deleted>>> Handle(DeleteExperienceCommand request,
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
                description: "Access denied. Only members can add education.");
        }

        if (currentUser.UserId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
        }


        if (currentUser.UserId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: $"Access denied. The MemberId you entered is invalid: {request.MemberId}");
        }

        var experience = await experienceRepository.Entites()
            .FirstOrDefaultAsync(x =>
                    x.MemberId == request.MemberId &&
                    x.Id == request.ExperienceId,
                cancellationToken);

        if (experience is null)
        {
            return Error.NotFound(
                code: "Experience.NotFound",
                description: "Experience not found for the specified ID and user.");
        }

        // Attempt deletion
        var deletionResult = await unitOfWork.Experiences.DeleteAsync(experience);
        if (!deletionResult)
        {
            return Error.Failure(
                code: "Experience.Failure",
                description: "Failed to delete experience.");
        }

        await unitOfWork.CompleteAsync(cancellationToken);

        return new ApiResponse<Deleted>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.NoContent,
            Message = "Experience deleted successfully.",
            Data = null
        };
    }
}