using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.savedJobs.Queries.GetSavedJobs;

public sealed class GetSavedJobsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
    : IRequestHandler<GetSavedJobsQuery, ErrorOr<ApiResponse<IReadOnlyList<SavedJobsResponse>>>>
{
    public async Task<ErrorOr<ApiResponse<IReadOnlyList<SavedJobsResponse>>>>
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
            .Include(s => s.Job)
            .ThenInclude(j => j.Questions)
            .Where(s => s.MemberId == request.MemberId)
            .Select(s => new SavedJobsResponse(
                s.Id,
                s.JobId,
                s.Job.Title,
                s.Job.Company.CompanyName,
                s.SavedOn,
                s.Job.CompanyId,
                s.Job.Title,
                s.Job.Description,
                s.Job.Location,
                s.Job.PostedOn,
                s.Job.SalaryTypeId,
                s.Job.Salary,
                s.Job.JobTypeId,
                s.Job.Questions.Select(q => new JobQuestionResponse(
                    q.Id,
                    q.QuestionText,
                    (int)q.QuestionType
                )).ToList()
            ))
            .ToListAsync(cancellationToken);

        return new ApiResponse<IReadOnlyList<SavedJobsResponse>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "SavedJobs Is Retrevied successfully.",
            Data = savedJobs
        };
    }
}