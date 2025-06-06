using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using Mapster;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.savedJobs.Queries.GetSavedJobs;

public sealed class GetSavedJobsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
    : IRequestHandler<GetSavedJobsQuery, ErrorOr<ApiResponse<IReadOnlyList<GetAllSavedJobsResponse>>>>
{
    public async Task<ErrorOr<ApiResponse<IReadOnlyList<GetAllSavedJobsResponse>>>>
        Handle(GetSavedJobsQuery request, CancellationToken cancellationToken)
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

        if (request.MemberId != currentUser.UserId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: $"Access denied. Id you entered : {request.MemberId} is wrong.");
        }

        var savedJobs = await unitOfWork.SavedJobs
            .Entites()
            .Include(s => s.Job)
            .ThenInclude(j => j.Company)
            .Where(s => s.MemberId == request.MemberId)
            .ProjectToType<GetAllSavedJobsResponse>()
            .ToListAsync(cancellationToken);

        return new ApiResponse<IReadOnlyList<GetAllSavedJobsResponse>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Saved jobs retrieved successfully.",
            Data = savedJobs
        };
    }
}