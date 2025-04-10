using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.accomplishment.Commands.DeleteAccomplishment
{
    public sealed class DeleteAccomplishmentCommandHandler : IRequestHandler<DeleteAccomplishmentCommand, ErrorOr<ApiResponse<Deleted>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Accomplishment> _accomplishmentRepository;

        public DeleteAccomplishmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Accomplishment> accomplishmentRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _accomplishmentRepository = accomplishmentRepository;
        }

        public async Task<ErrorOr<ApiResponse<Deleted>>> Handle(DeleteAccomplishmentCommand request, CancellationToken cancellationToken)
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
                    description: "Access denied. Only members can delete accomplishments."
                );
            }

            if (currentUser.UserId != request.MemberId)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}"
                );
            }

            var accomplishment = await _accomplishmentRepository.Entites()
                .Where(x => x.MemberId == currentUser.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (accomplishment is null)
            {
                return Error.NotFound(
                    code: "Accomplishment.NotFound",
                    description: "No accomplishment found for the current user."
                );
            }

            var deletionResult = await _unitOfWork.Accomplishments.DeleteAsync(accomplishment);
            if (!deletionResult)
            {
                return Error.Failure(
                    code: "Accomplishment.Failure",
                    description: "Failed to delete accomplishment for the current user."
                );
            }

            await _unitOfWork.CompleteAsync(cancellationToken);
            return new ApiResponse<Deleted>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.NoContent,
                Message = "Accomplishment deleted successfully.",
                Data = null
            };
        }
    }
}

