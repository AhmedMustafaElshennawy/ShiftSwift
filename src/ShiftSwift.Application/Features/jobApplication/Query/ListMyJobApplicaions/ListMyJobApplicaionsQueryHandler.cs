using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.jobApplication.Query.ListMyJobApplicaions;

public sealed class ListMyJobApplicaionsQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<ListMyJobApplicaionsQuery, ErrorOr<ApiResponse<IReadOnlyList<ListMyJobApplicaionsResponse>>>>
{
    public async Task<ErrorOr<ApiResponse<IReadOnlyList<ListMyJobApplicaionsResponse>>>> Handle(ListMyJobApplicaionsQuery request, CancellationToken cancellationToken)
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
                description: $"Access denied. Id you entered :{request.MemberId} is wrong.");
        }

        //var jobApplications = await _unitOfWork.JobApplications.Entites()
        //    .Where(j => j.MemberId == currentUser.UserId)
        //    .ToListAsync(cancellationToken);

        var jobApplications = await unitOfWork.JobApplications.Entites()
            .Where(j => j.MemberId == currentUser.UserId)
            .Select(j => new ListMyJobApplicaionsResponse(
                j.Job.Id,
                j.Job.Title,
                j.Job.Description,
                j.Job.Location,
                j.Job.PostedOn
            ))
            .ToListAsync(cancellationToken);

        return new ApiResponse<IReadOnlyList<ListMyJobApplicaionsResponse>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Job applications retrieved successfully.",
            Data = jobApplications
        };
    }
}