using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.ProfileData.Commands.ChangeMemberEmail;

public sealed class ChangeMemberEmailCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<ChangeMemberEmailCommand, ErrorOr<ApiResponse<MemberResponse>>>
{
    public async Task<ErrorOr<ApiResponse<MemberResponse>>> Handle(ChangeMemberEmailCommand request, CancellationToken cancellationToken)
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
        var member = await unitOfWork.Members.Entites()
            .Where(M => M.Id == request.MemberId)
            .SingleOrDefaultAsync(cancellationToken);

        if (member is null)
        {
            return Error.NotFound(
                code: "User.NotFound",
                description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
        }

        member.Email = request.Email;
        await unitOfWork.Members.UpdateAsync(member);
        await unitOfWork.CompleteAsync(cancellationToken);


        var MemberResponse = new MemberResponse(member.Id,
            member.FullName,
            member.UserName!,
            member.PhoneNumber!,
            member.Email!,
            member.GenderId.Value,
            member.Location);

        return new ApiResponse<MemberResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "User Profile Data Added successfully",
            Data = MemberResponse
        };
    }
}