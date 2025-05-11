using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetSpecificApplicant
{
    public sealed class GetSpecificApplicantQueryHandler : IRequestHandler<GetSpecificApplicantQuery, ErrorOr<ApiResponse<MemberResponseInfo>>>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Member> _memberRepository;

        public GetSpecificApplicantQueryHandler(ICurrentUserProvider currentUserProvider, IBaseRepository<Member> memberRepository)
        {
            _currentUserProvider = currentUserProvider;
            _memberRepository = memberRepository;
        }

        public async Task<ErrorOr<ApiResponse<MemberResponseInfo>>> Handle(GetSpecificApplicantQuery request, CancellationToken cancellationToken)
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
                    description: "companies can access applicant details.");
            }

            var applicant = await _memberRepository.Entites()
                .AsNoTracking()
                .Include(m => m.Educations)
                .Include(m => m.Experiences)
                .Include(m => m.Skills)
                .Include(m => m.JobApplications)
                    .ThenInclude(ja => ja.Job)
                .Where(m => m.Id == request.MemberId &&
                            m.JobApplications.Any(ja => ja.JobId == request.JobId && ja.Job.CompanyId == currentUserResult.Value.UserId))
                .FirstOrDefaultAsync(cancellationToken);

            if (applicant is null)
                return new ApiResponse<MemberResponseInfo>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Applicant not found or not authorized."
                };

            var educationResponses = applicant.Educations.Select(e => new MemberEducationResponse(
                e.Id,
                e.SchoolName,
                e.LevelOfEducation,
                e.FieldOfStudy
            )).ToList();

            var experienceResponses = applicant.Experiences.Select(e => new MemberExperienceResponse(
                e.Title,
                e.CompanyName,
                e.StartDate,
                e.EndDate,
                e.Description
            )).ToList();

            var skillResponses = applicant.Skills.Select(s => new MemberSkillResponse(
                s.Name
            )).ToList();

            var response = new MemberResponseInfo(
                applicant.Id,
                applicant.FullName,
                applicant.UserName!,
                applicant.PhoneNumber!,
                applicant.Email!,
                applicant.GenderId.HasValue ? (int)applicant.GenderId.Value : 0,
                applicant.Location ?? string.Empty,
                educationResponses,
                experienceResponses,
                skillResponses
            );

            return new ApiResponse<MemberResponseInfo>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Applicant details retrieved successfully.",
                Data = response
            };
        }
    }
}
