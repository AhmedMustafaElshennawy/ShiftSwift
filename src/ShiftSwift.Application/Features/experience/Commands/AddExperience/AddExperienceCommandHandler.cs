using ErrorOr;
using MediatR;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.experience.Commands.AddExperience;

public sealed class AddExperienceCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<AddExperienceCommand, ErrorOr<ApiResponse<AddExperienceResponse>>>
{
    public async Task<ErrorOr<ApiResponse<AddExperienceResponse>>> Handle(
        AddExperienceCommand request,
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
                description: "Access denied. Only members can add experiences.");
        }

        if (currentUser.UserId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "Access denied. Invalid MemberId.");
        }

        var newExperience = new Experience
        {
            Id = Guid.NewGuid(),
            MemberId = currentUser.UserId,
            Title = request.Title,
            CompanyName = request.CompanyName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Description = request.Description
        };

        await unitOfWork.Experiences.AddEntityAsync(newExperience);
        await unitOfWork.CompleteAsync(cancellationToken);

        var response = new AddExperienceResponse(
            newExperience.Id,
            currentUser.UserId,
            request.Title,
            request.CompanyName,
            request.StartDate,
            request.EndDate,
            request.Description);

        return new ApiResponse<AddExperienceResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "Experience added successfully.",
            Data = response
        };
    }
}