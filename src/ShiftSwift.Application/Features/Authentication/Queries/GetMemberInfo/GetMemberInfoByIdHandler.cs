using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.identity;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetMemberInfo;

public sealed class GetMemberInfoByIdHandler(
    UserManager<Account> userManager,
    IBaseRepository<Member> repository)
    : IRequestHandler<GetMemberInfoById, ErrorOr<ApiResponse<MemberResponseInfo>>>
{
    public async Task<ErrorOr<ApiResponse<MemberResponseInfo>>> Handle(GetMemberInfoById request,
        CancellationToken cancellationToken)
    {
        var memberData = await repository.Entites()
            .Include(m => m.Educations)
            .Include(m => m.Experiences)
            .Include(m => m.Skills)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (memberData is null)
        {
            return Error.NotFound(
                code:"User.NotFound", 
                description:"User not found.");
        }

        var isMember = await userManager.IsInRoleAsync(memberData, "Member");
        if (!isMember)
        {
            return Error.NotFound(
                code: "Member.NotFound",
                description: "User is not a member.");
        }


        var response = memberData.Adapt<MemberResponseInfo>();
        return new ApiResponse<MemberResponseInfo>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Member profile retrieved successfully.",
            Data = response
        };
    }
}
