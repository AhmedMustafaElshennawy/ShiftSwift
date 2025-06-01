using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Application.Features.education.Commands.DeleteEducation;

public sealed class DeleteEducationCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Education> educationRepository)
    : IRequestHandler<DeleteEducationCommand, ErrorOr<ApiResponse<Deleted>>>
{
    public async Task<ErrorOr<ApiResponse<Deleted>>> Handle(DeleteEducationCommand request,
        CancellationToken cancellationToken)
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
                description: "Access denied. Only members can add education.");
        }

        if (currentUser.UserId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
        }

        var currentUserEducation = await educationRepository.Entites()
            .Where(x => x.MemberId == currentUser.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (currentUserEducation is null)
        {
            return Error.NotFound(
                code: "Education.NotFound",
                description: "No Education Found to Current User.");
        }

        var deletionResult = await unitOfWork.Educations.DeleteAsync(currentUserEducation);
        if (deletionResult is not true)
        {
            return Error.Failure(
                code: "Education.Failure",
                description: "Failed To Delete Education For Current User.");
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        return new ApiResponse<Deleted>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.NoContent,
            Message = "Education Deleted successfully.",
            Data = null
        };
    }
}