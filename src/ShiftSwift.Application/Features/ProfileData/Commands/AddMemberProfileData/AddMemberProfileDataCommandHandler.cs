using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.ApiResponse;
using System.Net;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddMemberProfileData;

public sealed class AddMemberProfileDataCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<AddMemberProfileDataCommand, ErrorOr<ApiResponse<AddMemberProfileDataResponse>>>
{
    public async Task<ErrorOr<ApiResponse<AddMemberProfileDataResponse>>> Handle(
        AddMemberProfileDataCommand request, CancellationToken cancellationToken)
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

        var member = await unitOfWork.Members.Entites()
            .Where(M => M.Id == currentUser.UserId)
            .SingleOrDefaultAsync(cancellationToken);

        if (member is null)
        {
            return Error.NotFound(
                code: "User.NotFound",
                description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
        }

        member.FirstName = request.FirstName;
        member.LastName = request.LastName;
        member.GenderId = request.GenderId;
        member.Location = request.Location;
        member.PhoneNumber = request.PhoneNumber;
        member.AlternativeNumber = request.AlternativeNumber;
        member.BirthDate = request.DateOfBirth;

        await unitOfWork.Members.UpdateAsync(member);
        await unitOfWork.CompleteAsync(cancellationToken);

        var response = member.Adapt<AddMemberProfileDataResponse>();

        return new ApiResponse<AddMemberProfileDataResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "User profile data updated successfully.",
            Data = response
        };
    }
}