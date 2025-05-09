using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.experience.Queries.GetExperience;

public sealed class GetExperienceQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Experience> experienceRepository)
    : IRequestHandler<GetExperienceQuery, ErrorOr<ApiResponse<List<ExperienceResponse>>>>
{
    public async Task<ErrorOr<ApiResponse<List<ExperienceResponse>>>> Handle(
        GetExperienceQuery request,
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
                description: "Access denied. Only members can view experiences.");
        }

        if (currentUser.UserId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "Access denied. Invalid MemberId.");
        }

        var experiences = await experienceRepository.Entites()
            .Where(x => x.MemberId == currentUser.UserId)
            .ToListAsync(cancellationToken);

        var experienceResponses = experiences.Select(exp => new ExperienceResponse(
                currentUser.UserId,
                exp.Title,
                exp.CompanyName,
                exp.StartDate,
                exp.EndDate,
                exp.Description))
            .ToList();

        return new ApiResponse<List<ExperienceResponse>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = experiences.Any()
                ? "Experiences retrieved successfully."
                : "No experiences found for this member.",
            Data = experienceResponses
        };
    }
}