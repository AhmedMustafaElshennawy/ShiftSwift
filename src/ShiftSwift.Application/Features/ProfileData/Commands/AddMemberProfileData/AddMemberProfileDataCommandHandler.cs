using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddMemberProfileData
{
    public sealed class AddMemberProfileDataCommandHandler : IRequestHandler<AddMemberProfileDataCommand, ErrorOr<ApiResponse<MemberResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        public AddMemberProfileDataCommandHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<ErrorOr<ApiResponse<MemberResponse>>> Handle(AddMemberProfileDataCommand request, CancellationToken cancellationToken)
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
            member.MiddleName = request.MeddileName;
            member.GenderId = request.GenderId;

            await _unitOfWork.Members.UpdateAsync(member);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var MemberResponse = new MemberResponse(member.Id,
              member.FullName,
              member.UserName!,
              member.PhoneNumber!,
              member.Email!,
              member.GenderId);

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
