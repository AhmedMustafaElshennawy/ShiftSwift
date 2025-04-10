using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.education.Queries.GetEducation
{
    public sealed class GetEducationQueryHandler : IRequestHandler<GetEducationQuery, ErrorOr<ApiResponse<EducationRespone>>>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Education> _educationRepository;
        public GetEducationQueryHandler(ICurrentUserProvider currentUserProvider,IBaseRepository<Education> educationRepository)
        {
            _currentUserProvider = currentUserProvider;
            _educationRepository = educationRepository;
        }
        public async Task<ErrorOr<ApiResponse<EducationRespone>>> Handle(GetEducationQuery request, CancellationToken cancellationToken)
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
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
            }

            var currentUserEducation = await _educationRepository.Entites()
                    .Where(x => x.MemberId == currentUser.UserId)
                    .FirstOrDefaultAsync(cancellationToken);

            if (currentUserEducation is null)
            {
                return new ApiResponse<EducationRespone>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Education Rturned successfully.",
                    Data = "The Current Has No Education"
                };
            }

            var educationResponse = new EducationRespone(
                currentUserEducation.Id,
                currentUserEducation.SchoolName,
                currentUserEducation.LevelOfEducation,
                currentUserEducation.FieldOfStudy);

            return new ApiResponse<EducationRespone>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Education Rturned successfully.",
                Data = educationResponse
            };
        }
    }
}