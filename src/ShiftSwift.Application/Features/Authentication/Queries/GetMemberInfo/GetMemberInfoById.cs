using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;
using System.Security.Principal;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetMemberInfo
{
    public sealed record GetMemberInfoById(string Id) : IRequest<ErrorOr<ApiResponse<MemberResponseInfo>>>;

    public sealed class GetMemberInfoByIdHandler : IRequestHandler<GetMemberInfoById, ErrorOr<ApiResponse<MemberResponseInfo>>>
    {
        private readonly IBaseRepository<Member> _repository;
        private readonly UserManager<Account> _userManager;
        public GetMemberInfoByIdHandler(UserManager<Account> userManager, IBaseRepository<Member> repository)
        {
            _userManager = userManager;
            _repository = repository;
        }
        public async Task<ErrorOr<ApiResponse<MemberResponseInfo>>> Handle(GetMemberInfoById request, CancellationToken cancellationToken)
        {
            var member = await _repository.Entites()
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (member is null)
            {
                return Error.NotFound("User.NotFound", "User not found.");
            }

            var isMember = await _userManager.IsInRoleAsync(member, "Member");
            if (!isMember)
            {
                return Error.NotFound(
                    code:"Member.NotFound",
                    description: "User is not a member.");
            }


            if (member is null)
            {
                return Error.NotFound(
                    code: "User.NotFound",
                    description: "User not found.");
            }

            var response = new MemberResponseInfo(
                memberId: member.Id,
                FullName: member.FullName,
                UserName: member.UserName!,
                PhoneNumber: member.PhoneNumber!,
                Email: member.Email!,
                GenderId: member.GenderId.Value,
                Location: member.Location
            );

            return new ApiResponse<MemberResponseInfo>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Member profile retrieved successfully.",
                Data = response
            };
        }
    }
}