using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetMemberInfo;

public sealed record GetMemberInfoById(string Id) : IRequest<ErrorOr<ApiResponse<MemberResponseInfo>>>;

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
            return Error.NotFound("User.NotFound", "User not found.");
        }

        var isMember = await userManager.IsInRoleAsync(memberData, "Member");
        if (!isMember)
        {
            return Error.NotFound(
                code: "Member.NotFound",
                description: "User is not a member.");
        }

        var educationResponses = memberData.Educations.Select(e => new MemberEducationResponse(
            e.Id,
            e.Level,
            e.Faculty,
            e.UniversityName
        )).ToList();

        var experienceResponses = memberData.Experiences.Select(e => new MemberExperienceResponse(
            e.Title,
            e.CompanyName,
            e.StartDate,
            e.EndDate,
            e.Description
        )).ToList();

        var skillResponses = memberData.Skills.Select(s => new MemberSkillResponse(
            s.Name
        )).ToList();

        var response = new MemberResponseInfo(
            MemberId: memberData.Id,
            FullName: memberData.FullName,
            UserName: memberData.UserName!,
            PhoneNumber: memberData.PhoneNumber!,
            Email: memberData.Email!,
            GenderId: memberData.GenderId!.Value,
            Location: memberData.Location,
            Educations: educationResponses,
            Experiences: experienceResponses,
            Skills: skillResponses
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