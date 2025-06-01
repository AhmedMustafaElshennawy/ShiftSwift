using ErrorOr;
using MediatR;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.shared;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplicationWithoutQuestion;

public sealed class CreateJobApplicationCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<JobApplication> jobApplicationRepository)
    : IRequestHandler<CreateJobApplicationWithoutJobQuestionCommand, ErrorOr<ApiResponse<JobApplicationResponse>>>
{
    public async Task<ErrorOr<ApiResponse<JobApplicationResponse>>> Handle(CreateJobApplicationWithoutJobQuestionCommand request, CancellationToken cancellationToken)
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
                description: "Access denied. Only members can add Job Application.");
        }

        var job = await unitOfWork.Jobs.Entites()
            .Where(J => J.Id == request.JobId)
            .FirstOrDefaultAsync(cancellationToken);

        if (job is null)
        {
            return Error.NotFound(
                code: "Job.NotFound",
                description: $"No Job Found With Id You Entred {request.JobId}.");
        }

        var memberId = request.MemberId ?? currentUser.UserId;
        var existingApplication = await jobApplicationRepository.Entites()
            .AsQueryable()
            .AnyAsync(ja => ja.JobId == request.JobId && ja.MemberId == memberId, cancellationToken);

        if (existingApplication)
        {
            return Error.Conflict(
                code: "JobApplication.Duplicate",
                description: "You have already applied for this job.");
        }

        var jobApplicaion = new JobApplication
        {
            Id = Guid.NewGuid(),
            AppliedOn = DateTime.UtcNow,
            JobId = request.JobId,
            MemberId = request.MemberId ?? currentUser.UserId,
            Status = 1
        };
        await jobApplicationRepository.AddEntityAsync(jobApplicaion);
        await unitOfWork.CompleteAsync(cancellationToken);

        var response = new JobApplicationWthoutQuestionResponse(jobApplicaion.Id,
            jobApplicaion.JobId,
            jobApplicaion.MemberId,
            jobApplicaion.AppliedOn);

        return new ApiResponse<JobApplicationResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "JobApplication added successfully.",
            Data = response
        };
    }
}