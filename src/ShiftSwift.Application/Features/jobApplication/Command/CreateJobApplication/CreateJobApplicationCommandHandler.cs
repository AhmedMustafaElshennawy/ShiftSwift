using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.shared;
using System.Net;
using Mapster;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplication;

internal sealed class CreateJobApplicationCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<JobApplication> jobApplicationRepository)
    : IRequestHandler<CreateJobApplicationCommand, ErrorOr<ApiResponse<AddJobApplicationResponse>>>
{
    public async Task<ErrorOr<ApiResponse<AddJobApplicationResponse>>> Handle(CreateJobApplicationCommand request,
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
                description: "Access denied. Only members can apply for jobs.");
        }

        var job = await unitOfWork.Jobs.Entites()
            .Where(j => j.Id == request.JobId)
            .FirstOrDefaultAsync(cancellationToken);

        if (job is null)
        {
            return Error.NotFound(
                code: "Job.NotFound",
                description: $"No Job Found With Id You Entred {request.JobId}.");
        }

        var existingApplication = await jobApplicationRepository.Entites()
            .AsQueryable()
            .AnyAsync(ja => ja.JobId == request.JobId && ja.MemberId == currentUser.UserId, cancellationToken);

        if (existingApplication)
        {
            return Error.Conflict(
                code: "JobApplication.Duplicate",
                description: "You have already applied for this job.");
        }

        var jobApplication = new JobApplication
        {
            Id = Guid.NewGuid(),
            AppliedOn = DateTime.UtcNow,
            JobId = request.JobId,
            MemberId = currentUser.UserId,
            Status = 1
        };

        await unitOfWork.JobApplications.AddEntityAsync(jobApplication);
        await unitOfWork.CompleteAsync(cancellationToken);

        var response = jobApplication.Adapt<AddJobApplicationResponse>();

        return new ApiResponse<AddJobApplicationResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "JobApplication added successfully.",
            Data = response
        };
    }
}