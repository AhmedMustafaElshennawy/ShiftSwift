using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.job.Commands.ApplyApplicant
{
    public sealed class ApplyApplicantCommandHandler : IRequestHandler<ApplyApplicantCommand, ErrorOr<ApiResponse<ApplyApplicantResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Member> _MemberRepository;
        public ApplyApplicantCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Member> memberRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _MemberRepository = memberRepository;
        }
        public async Task<ErrorOr<ApiResponse<ApplyApplicantResponse>>> Handle(ApplyApplicantCommand request, CancellationToken cancellationToken)
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
                    description: "Access denied. Only Company Accept Applicants.");
            }

            var appliedMember = await _MemberRepository.Entites()
                .Where(m => m.Id == request.MemberId)
                .FirstOrDefaultAsync(cancellationToken);

            if (appliedMember is null)
            {
                return Error.NotFound(
                    code: "Member.NotFound",
                    description: "The member does not exist.");
            }

            var jobApplication = await _unitOfWork.JobApplications.Entites()
                .Where(ja => ja.JobId == request.JobId && ja.MemberId == request.MemberId)
                .FirstOrDefaultAsync(cancellationToken);

            if (jobApplication is null)
            {
                return Error.NotFound(
                    code: "JobApplication.NotFound",
                    description: "No job application found for this job and member.");
            }

            jobApplication.Status = true;

            await _unitOfWork.JobApplications.UpdateAsync(jobApplication);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var applyApplicantResponse = new ApplyApplicantResponse(appliedMember.Id,
                appliedMember.FullName,
                appliedMember.UserName!,
                appliedMember.PhoneNumber!,
                appliedMember.Email!,
                jobApplication.Status);

            return new ApiResponse<ApplyApplicantResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Member Is Accepted To Yor JobApplicaion successfully.",
                Data = applyApplicantResponse
            };
        }
    }
}
