using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.ProfileData.Commands.ChangeMemberEmail
{
    public sealed class ChangeMemberEmailCommandHandler : IRequestHandler<ChangeMemberEmailCommand, ErrorOr<ApiResponse<MemberResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        public ChangeMemberEmailCommandHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<ErrorOr<ApiResponse<MemberResponse>>> Handle(ChangeMemberEmailCommand request, CancellationToken cancellationToken)
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
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
            }
            var member = await _unitOfWork.Members.Entites()
                .Where(M => M.Id == request.MemberId)
                .SingleOrDefaultAsync(cancellationToken);

            if (member is null)
            {
                return Error.NotFound(
                    code: "User.NotFound",
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
            }

            member.Email = request.Email;
            await _unitOfWork.Members.UpdateAsync(member);
            await _unitOfWork.CompleteAsync(cancellationToken);


            var MemberResponse = new MemberResponse(member.Id,
               member.FullName,
               member.UserName!,
               member.PhoneNumber!,
               member.Email!,
               member.GenderId,
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
}