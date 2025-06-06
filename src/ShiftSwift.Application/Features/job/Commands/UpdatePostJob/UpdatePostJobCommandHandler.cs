using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using Mapster;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Commands.UpdatePostJob;

public sealed class UpdatePostJobCommandHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
    : IRequestHandler<UpdatePostJobCommand, ErrorOr<ApiResponse<UpdatePostedJobResponse>>>
{
    public async Task<ErrorOr<ApiResponse<UpdatePostedJobResponse>>> Handle(UpdatePostJobCommand request,
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
        if (!currentUser.Roles.Contains("Company"))
        {
            return Error.Forbidden(
                code: "User.Forbidden",
                description: "Access denied. Only companies can update job posts");
        }

        var existingJob = await unitOfWork.Jobs.Entites()
            .Where(j => j.Id == request.JobId)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingJob is null)
        {
            return new ApiResponse<UpdatePostedJobResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = $"No Jobs Found To Your Company With Id You Entered: {request.JobId}",
                Data = null
            };
        }

        if (existingJob.CompanyId != currentUser.UserId)
        {
            return Error.Forbidden(
                code: "Job.Forbidden",
                description: "You are not allowed to Update this job.");
        }

        var job = await unitOfWork.Jobs.Entites()
            .FirstOrDefaultAsync(j => j.Id == request.JobId, cancellationToken);

        if (job is null)
        {
            return new ApiResponse<UpdatePostedJobResponse>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = $"No job found with Id: {request.JobId}"
            };
        }

        if (job.CompanyId != currentUser.UserId)
        {
            return Error.Forbidden(
                code: "Job.Forbidden",
                description: "You are not allowed to update this job.");
        }

        job.Title = request.Title;
        job.Description = request.Description;
        job.Location = request.Location;
        job.JobTypeId = request.JobType;
        job.WorkModeId = request.WorkMode;
        job.Salary = request.Salary;
        job.SalaryTypeId = request.SalaryType;
        job.Requirements = request.Requirements;
        job.Keywords = request.Keywords;

        await unitOfWork.Jobs.UpdateAsync(job);
        await unitOfWork.CompleteAsync(cancellationToken);

        var response = job.Adapt<UpdatePostedJobResponse>();
        return new ApiResponse<UpdatePostedJobResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Job updated successfully.",
            Data = response
        };
    }
}