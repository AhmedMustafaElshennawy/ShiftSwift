using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.savedJobs.Queries.GetSavedJobs
{
    public sealed class GetSavedJobsQueryHandler : IRequestHandler<GetSavedJobsQuery, ErrorOr<ApiResponse<IReadOnlyList<SavedJobsResponse>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        public GetSavedJobsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<ErrorOr<ApiResponse<IReadOnlyList<SavedJobsResponse>>>> Handle(GetSavedJobsQuery request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
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

            var savedJobs = await _unitOfWork.SavedJobs
                .Entites()
                .Include(s => s.Job)
                .ThenInclude(j => j.Company)
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
                    s.Job.JobTypeId))
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
}
