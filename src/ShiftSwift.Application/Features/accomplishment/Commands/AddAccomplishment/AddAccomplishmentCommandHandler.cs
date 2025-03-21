using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.accomplishment.Commands.AddAccomplishment
{
    public sealed class AddAccomplishmentCommandHandler : IRequestHandler<AddAccomplishmentCommand, ErrorOr<ApiResponse<AccomplishmentResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Accomplishment> _accomplishmentRepository;

        public AddAccomplishmentCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Accomplishment> accomplishmentRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _accomplishmentRepository = accomplishmentRepository;
        }

        public async Task<ErrorOr<ApiResponse<AccomplishmentResponse>>> Handle(AddAccomplishmentCommand request, CancellationToken cancellationToken)
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
                    description: "Access denied. Only members can add accomplishments.");
            }

            if (currentUser.UserId != request.MemberId)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
            }

            var currentUserAccomplishment = await _accomplishmentRepository.Entites()
                .Where(x => x.MemberId == currentUser.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            bool isUpdate = currentUserAccomplishment is not null;
            if (isUpdate)
            {
                currentUserAccomplishment!.Title = request.Title;
                currentUserAccomplishment.Description = request.Description;
                currentUserAccomplishment.DateAchieved = request.DateAchieved;

                await _unitOfWork.Accomplishments.UpdateAsync(currentUserAccomplishment);
            }
            else
            {
                currentUserAccomplishment = new Accomplishment
                {
                    Id = Guid.NewGuid(),
                    MemberId = currentUser.UserId,
                    Title = request.Title,
                    Description = request.Description,
                    DateAchieved = request.DateAchieved
                };

                await _unitOfWork.Accomplishments.AddEntityAsync(currentUserAccomplishment);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            var response = new AccomplishmentResponse(
                currentUserAccomplishment.Id,
                currentUser.UserId,
                request.Title,
                request.Description,
                request.DateAchieved);

            return new ApiResponse<AccomplishmentResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = isUpdate ? "Accomplishment updated successfully." : "Accomplishment added successfully.",
                Data = response
            };
        }
    }
}

