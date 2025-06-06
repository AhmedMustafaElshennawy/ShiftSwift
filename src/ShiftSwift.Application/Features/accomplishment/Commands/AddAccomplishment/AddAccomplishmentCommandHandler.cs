using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Application.Features.accomplishment.Commands.AddAccomplishment;

public sealed class AddAccomplishmentCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Accomplishment> accomplishmentRepository)
    : IRequestHandler<AddAccomplishmentCommand, ErrorOr<ApiResponse<AccomplishmentResponse>>>
{
    public async Task<ErrorOr<ApiResponse<AccomplishmentResponse>>> Handle(AddAccomplishmentCommand request,
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
                description: "Access denied. Only members can add accomplishments.");
        }

        if (currentUser.UserId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
        }

        var currentUserAccomplishment = await accomplishmentRepository.Entites()
            .Where(x => x.MemberId == currentUser.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        bool isUpdate = currentUserAccomplishment is not null;

        if (isUpdate)
        {
            currentUserAccomplishment!.Title = request.Title;
            currentUserAccomplishment.Description = request.Description;
            currentUserAccomplishment.DateAchieved = request.DateAchieved;

            await unitOfWork.Accomplishments.UpdateAsync(currentUserAccomplishment);
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

            await unitOfWork.Accomplishments.AddEntityAsync(currentUserAccomplishment);
        }

        await unitOfWork.CompleteAsync(cancellationToken);

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