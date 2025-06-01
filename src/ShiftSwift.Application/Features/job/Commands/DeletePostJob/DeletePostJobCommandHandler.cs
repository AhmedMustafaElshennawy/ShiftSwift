using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Commands.DeletePostJob;

public sealed class DeletePostJobCommandHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
    : IRequestHandler<DeletePostJobCommand, ErrorOr<ApiResponse<Deleted>>>
{
    public async Task<ErrorOr<ApiResponse<Deleted>>> Handle(DeletePostJobCommand request, CancellationToken cancellationToken)
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
                description: "Access denied. Only members can Delete Jobs");
        }

        var job = await unitOfWork.Jobs.Entites()
            .Include(j => j.JobApplications)
            .Include(j => j.Questions)
            .Where(j => j.Id == request.JobId)
            .FirstOrDefaultAsync(cancellationToken);

        if (job is null)
        {
            return new ApiResponse<Deleted>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = $"No Jobs Found To Your Company With Id You Entered: {request.JobId}",
                Data = null
            };
        }

        if (job.CompanyId != currentUser.UserId)
        {
            return Error.Forbidden(
                code: "Job.Forbidden",
                description: "You are not allowed to delete this job.");
        }

        foreach (var application in job.JobApplications)
        {
            await unitOfWork.JobApplications.DeleteAsync(application);
        }

        foreach (var question in job.Questions)
        {
            await unitOfWork.JobQuestions.DeleteAsync(question);
        }

        await unitOfWork.Jobs.DeleteAsync(job);
        await unitOfWork.CompleteAsync(cancellationToken);

        return new ApiResponse<Deleted>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Your Job is Deleted successfully.",
            Data = null
        };
    }
}