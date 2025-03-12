using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;


namespace ShiftSwift.Application.Features.education.Commands.AddEducation
{
    public sealed class AddEducationCommandHandler : IRequestHandler<AddEducationCommand, ErrorOr<ApiResponse<EducationRespone>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Education> _educationRepository;
        public AddEducationCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Education> educationRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _educationRepository = educationRepository;
        }
        public async Task<ErrorOr<ApiResponse<EducationRespone>>> Handle(AddEducationCommand request, CancellationToken cancellationToken)
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
            if (currentUser.UserId != request.MemberId)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: "Access denied. The MemberId Is Wrong");
            }

            var currentUserEducation = await _educationRepository.Entites()
                    .Where(x => x.MemberId == currentUser.UserId)
                    .FirstOrDefaultAsync(cancellationToken);

            bool isUpdate = currentUserEducation is not null;
            if (isUpdate)
            {
                currentUserEducation!.Institution = request.Institution;
                currentUserEducation.Degree = request.Degree;

                await _unitOfWork.Educations.UpdateAsync(currentUserEducation);
            }
            else
            {
                currentUserEducation = new Education
                {
                    Id = Guid.NewGuid(),
                    MemberId = currentUser.UserId,
                    Institution = request.Institution,
                    Degree = request.Degree
                };

                await _unitOfWork.Educations.AddEntityAsync(currentUserEducation);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);
            var educationResponse = new EducationRespone(
                currentUserEducation.Id,
                currentUser.UserId,
                request.Institution,
                request.Degree);

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
