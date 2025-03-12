using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetApplicants
{
    public sealed class GetApplicantsQueryHandler : IRequestHandler<GetApplicantsQuery, ErrorOr<ApiResponse<IReadOnlyList<GetApplicantsResponse>>>>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Member> _memberRepository;
        public GetApplicantsQueryHandler(ICurrentUserProvider currentUserProvider, IBaseRepository<Member> memberRepository)
        {
            _currentUserProvider = currentUserProvider;
            _memberRepository = memberRepository;
        }
        public async Task<ErrorOr<ApiResponse<IReadOnlyList<GetApplicantsResponse>>>> Handle(GetApplicantsQuery request, CancellationToken cancellationToken)
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
                    description: "Access denied. Only Company can add education.");
            }

            var response = await _memberRepository.Entites()
            .AsQueryable()
            .AsNoTracking()
            .Include(m => m.JobApplications)
            .ThenInclude(ja => ja.Job) // Include Job to access CompanyId
            .Where(m => m.JobApplications.Any(ja =>
                 ja.JobId == request.JobId && ja.Job.CompanyId == currentUser.UserId)) // Filter by JobId & CompanyId
            .Select(m => new GetApplicantsResponse(
                m.Id,
                m.FullName,
                m.UserName!,
                m.PhoneNumber!,
                m.Email!
            )).ToListAsync(cancellationToken);

            if (!response.Any())
            {
                return new ApiResponse<IReadOnlyList<GetApplicantsResponse>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "No applicants found.",
                    Data = Array.Empty<GetApplicantsResponse>()
                };
            }

            return new ApiResponse<IReadOnlyList<GetApplicantsResponse>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Applicants retrieved successfully.",
                Data = response
            };
        }
    }
}
