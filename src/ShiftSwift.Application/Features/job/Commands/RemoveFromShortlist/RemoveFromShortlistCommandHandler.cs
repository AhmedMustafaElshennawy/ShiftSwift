using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.Enums;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.job.Commands.RemoveFromShortlist
{
    public sealed class RemoveFromShortlistCommandHandler : IRequestHandler<RemoveFromShortlistCommand, ErrorOr<ApiResponse<string>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;

        public RemoveFromShortlistCommandHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<ErrorOr<ApiResponse<string>>> Handle(RemoveFromShortlistCommand request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
            if (currentUserResult.IsError || !currentUserResult.Value.Roles.Contains("Company"))
            {
                return Error.Forbidden(
                    code: "User.Forbidden",
                    description: "Only companies can remove applicants from shortlist.");
            }

            var job = await _unitOfWork.Jobs.Entites()
                .Where(j => j.Id == request.JobId && j.CompanyId == currentUserResult.Value.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (job is null)
            {
                return Error.Forbidden(
                    code: "Job.Forbidden",
                    description: "You are not authorized to remove a member from the shortlist.");
            }

            var jobApplication = await _unitOfWork.JobApplications.Entites()
                .Where(ja => ja.JobId == request.JobId
                          && ja.MemberId == request.MemberId
                          && ja.Status == (int)ApplicationStatus.Shortlisted)
                .FirstOrDefaultAsync(cancellationToken);

            if (jobApplication == null)
            {
                return Error.NotFound(
                    code: "JobApplication.NotFound",
                    description: "The specified member is not in the shortlist for this job.");
            }

            jobApplication.Status = (int)ApplicationStatus.RemovedFromShortlist;

            await _unitOfWork.JobApplications.UpdateAsync(jobApplication);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return new ApiResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Member successfully removed from shortlist.",
                Data = "Member removed from shortlist."
            };

        }
    }
}
