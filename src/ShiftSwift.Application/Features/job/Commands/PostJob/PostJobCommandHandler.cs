using ErrorOr;
using Mapster;
using MediatR;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.shared;
using System.Net;

namespace ShiftSwift.Application.Features.job.Commands.PostJob;

public sealed class PostJobCommandHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
    : IRequestHandler<PostJobCommand, ErrorOr<ApiResponse<CompanyPostedJobResponse>>>
{
    public async Task<ErrorOr<ApiResponse<CompanyPostedJobResponse>>> Handle(PostJobCommand request, CancellationToken cancellationToken)
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
                description: "Access denied. Only companies Can Add Job Posts");
        }

        var postJob = new Job
        {
            Id = Guid.NewGuid(),
            CompanyId = currentUser.UserId,
            Description = request.Description,
            Location = request.Location,
            PostedOn = DateTime.UtcNow,
            Title = request.Title,
            JobTypeId = request.JobType,
            WorkModeId = request.WorkMode,
            Salary = request.Salary,
            Requirements = request.Requirements,
            Keywords = request.Keywords,
            SalaryTypeId = request.SalaryType,
        };

        await unitOfWork.Jobs.AddEntityAsync(postJob);
        await unitOfWork.CompleteAsync(cancellationToken);

        var response = postJob.Adapt<CompanyPostedJobResponse>();
        return new ApiResponse<CompanyPostedJobResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "Job Posted successfully.",
            Data = response
        };
    }
}