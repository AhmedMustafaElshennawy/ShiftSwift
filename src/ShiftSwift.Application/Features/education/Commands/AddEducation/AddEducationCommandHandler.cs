using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;
using System.Security.Claims;


namespace ShiftSwift.Application.Features.education.Commands.AddEducation
{
    public sealed class AddEducationCommandHandler : IRequestHandler<AddEducationCommand, ErrorOr<ApiResponse<EducationRespone>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Education> _educationRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public AddEducationCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Education> educationRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _educationRepository = educationRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ErrorOr<ApiResponse<EducationRespone>>> Handle(AddEducationCommand request, CancellationToken cancellationToken)
        {
            var currentUserResult = _httpContextAccessor.HttpContext?.User;

            if (currentUserResult == null || !currentUserResult.Identity?.IsAuthenticated == true)
            {
                return Error.Failure(
                    code: "User.Failure",
                    description: "User is not authenticated."
                );
            }

            var userId = currentUserResult.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = currentUserResult.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = currentUserResult.FindFirst(ClaimTypes.Email)?.Value;
            var roles = currentUserResult.Claims
                                        .Where(c => c.Type == ClaimTypes.Role)
                                        .Select(c => c.Value)
                                        .ToList();
 
            if (!roles.Contains("Member"))
            {
                return Error.Forbidden(
                    code: "User.Forbidden",
                    description: "Access denied. Only members can add education.");
            }
            if (userId != request.MemberId)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: "Access denied. The MemberId Is Wrong");
            }

            var currentUserEducation = await _educationRepository.Entites()
                    .Where(x => x.MemberId == userId)
                    .FirstOrDefaultAsync(cancellationToken);

            bool isUpdate = currentUserEducation is not null;
            if (isUpdate)
            {
                currentUserEducation!.LevelOfEducation = request.LevelOfEducation;
                currentUserEducation.FieldOfStudy = request.FieldOfStudy;
                currentUserEducation.SchoolName = request.SchoolName;

                await _unitOfWork.Educations.UpdateAsync(currentUserEducation);
            }
            else
            {
                currentUserEducation = new Education
                {
                    Id = Guid.NewGuid(),
                    MemberId = userId,
                    SchoolName = request.SchoolName,
                    FieldOfStudy = request.FieldOfStudy,
                    LevelOfEducation = request.LevelOfEducation
                };

                await _unitOfWork.Educations.AddEntityAsync(currentUserEducation);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);
            var educationResponse = new EducationRespone(
                currentUserEducation.Id,
                request.SchoolName,
                request.LevelOfEducation,
                request.FieldOfStudy);

            return new ApiResponse<EducationRespone>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = isUpdate ? "Education updated successfully." : "Education added successfully.",
                Data = educationResponse
            };
        }
    }
}