using ErrorOr;
using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.experience.Commands.UpdateExperience;

public sealed class UpdateExperienceCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Experience> experienceRepository)
    : IRequestHandler<UpdateExperienceCommand, ErrorOr<ApiResponse<UpdateExperienceResponse>>>
{
    public async Task<ErrorOr<ApiResponse<UpdateExperienceResponse>>> Handle(
        UpdateExperienceCommand request,
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
                description: "Access denied. Only members can update experiences.");
        }

        if (currentUser.UserId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "Access denied. Invalid MemberId.");
        }

        var existingExperience = await experienceRepository.Entites()
            .FirstOrDefaultAsync(e =>
                    e.Id == request.ExperienceId &&
                    e.MemberId == currentUser.UserId,
                cancellationToken);

        if (existingExperience == null)
        {
            return Error.NotFound(
                code: "Experience.NotFound",
                description: "Experience not found or you don't have permission to update it.");
        }

        existingExperience.Title = request.Title;
        existingExperience.CompanyName = request.CompanyName;
        existingExperience.StartDate = request.StartDate;
        existingExperience.EndDate = request.EndDate;
        existingExperience.Description = request.Description;

        await unitOfWork.Experiences.UpdateAsync(existingExperience);
        await unitOfWork.CompleteAsync(cancellationToken);

        var response = new UpdateExperienceResponse(
            existingExperience.Id,
            currentUser.UserId,
            existingExperience.Title,
            existingExperience.CompanyName,
            existingExperience.StartDate,
            existingExperience.EndDate,
            existingExperience.Description);

        return new ApiResponse<UpdateExperienceResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Experience updated successfully.",
            Data = response
        };
    }
}