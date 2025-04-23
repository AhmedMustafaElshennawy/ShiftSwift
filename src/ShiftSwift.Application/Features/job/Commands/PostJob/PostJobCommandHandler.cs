using ErrorOr;
using MediatR;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.shared;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.job.Commands.PostJob
{
    public sealed class PostJobCommandHandler : IRequestHandler<PostJobCommand, ErrorOr<ApiResponse<PostedJobResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        public PostJobCommandHandler(IUnitOfWork unitOfWork,ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<ErrorOr<ApiResponse<PostedJobResponse>>> Handle(PostJobCommand request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
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
                    description: "Access denied. Only members Can Add Job Posts");
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
                SalaryTypeId = request.SalaryType
            };

            await _unitOfWork.Jobs.AddEntityAsync(postJob);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var response = new PostedJobResponse(currentUser.UserId,
                postJob.Id,
                postJob.Title,
                postJob.Description,
                postJob.Location,
                postJob.PostedOn,
                postJob.JobTypeId);

            return new ApiResponse<PostedJobResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Message = "Job Posted successfully.",
                Data = response
            };
        }
    }
}
