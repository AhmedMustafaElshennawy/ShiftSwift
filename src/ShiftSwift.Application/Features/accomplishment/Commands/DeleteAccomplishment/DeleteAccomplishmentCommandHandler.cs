using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.accomplishment.Commands.DeleteAccomplishment;

public sealed class DeleteAccomplishmentCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Accomplishment> accomplishmentRepository)
    : IRequestHandler<DeleteAccomplishmentCommand, ErrorOr<ApiResponse<Deleted>>>
{
    public async Task<ErrorOr<ApiResponse<Deleted>>> Handle(DeleteAccomplishmentCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = await currentUserProvider.GetCurrentUser();
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

        var accomplishment = await accomplishmentRepository.Entites()
            .Where(x => x.MemberId == currentUser.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (accomplishment is null)
        {
            return Error.NotFound(
                code: "Accomplishment.NotFound",
                description: "No accomplishment found for the current user."
            );
        }

        var deletionResult = await unitOfWork.Accomplishments.DeleteAsync(accomplishment);
        if (!deletionResult)
        {
            return Error.Failure(
                code: "Accomplishment.Failure",
                description: "Failed to delete accomplishment for the current user."
            );
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        return new ApiResponse<Deleted>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.NoContent,
            Message = "Accomplishment deleted successfully.",
            Data = null
        };
    }
}