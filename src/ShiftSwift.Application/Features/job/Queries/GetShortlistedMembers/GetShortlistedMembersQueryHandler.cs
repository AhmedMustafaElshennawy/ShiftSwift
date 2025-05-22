using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.Enums;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.job.Queries.GetShortlistedMembers
{
    public sealed class GetShortlistedMembersQueryHandler : IRequestHandler<GetShortlistedMembersQuery, ErrorOr<ApiResponse<List<ApplyApplicantResponse>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;

        public GetShortlistedMembersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<ErrorOr<ApiResponse<List<ApplyApplicantResponse>>>> Handle(GetShortlistedMembersQuery request, CancellationToken cancellationToken)
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
                    description: "Access denied. Only companies can manage applicants.");
            }

            var job = await _unitOfWork.Jobs.Entites()
               .Where(j => j.Id == request.JobId && j.CompanyId == currentUserResult.Value.UserId)
               .FirstOrDefaultAsync(cancellationToken);

            if (job is null)
            {
                return Error.Forbidden(
                    code: "Job.Forbidden",
                    description: "You are not authorized to Get Short listed Members.");
            }

            var shortlistedMembers = await _unitOfWork.JobApplications.Entites()
                .Where(ja => ja.JobId == request.JobId
                             && ja.Status == (int)ApplicationStatus.Shortlisted)
                .Join(_unitOfWork.Members.Entites(),
                      ja => ja.MemberId,
                      m => m.Id,
                      (ja, m) => new ApplyApplicantResponse(
                          m.Id,
                          m.FullName,
                          m.UserName!,
                          m.PhoneNumber!,
                          m.Email!,
                          ja.Status))
                .ToListAsync(cancellationToken);

            if (!shortlistedMembers.Any())
            {
                return Error.NotFound(
                    code: "ShortlistedMembers.NotFound",
                    description: "No shortlisted members found for this job and company.");
            }

            return new ApiResponse<List<ApplyApplicantResponse>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Shortlisted members retrieved successfully.",
                Data = shortlistedMembers
            };
        }
    }
}