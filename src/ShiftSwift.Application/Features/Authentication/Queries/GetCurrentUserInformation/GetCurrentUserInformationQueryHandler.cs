using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using System.Net;
using ShiftSwift.Domain.ApiResponse;


namespace ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserInformation;

public sealed class GetCurrentUserInformationQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Account> accountRepository)
    : IRequestHandler<GetCurrentUserInformationQuery, ErrorOr<ApiResponse<object>>>
{
    public async Task<ErrorOr<ApiResponse<object>>> Handle(GetCurrentUserInformationQuery request,
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

        var account = await accountRepository.Entites()
            .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken);

        if (account is null)
        {
            return Error.NotFound(
                code: "User.NotFound",
                description: "User not found.");
        }

        if (account is Company company)
        {
            var companyResponse = new CompanyResponse(currentUser.UserId,
                company.CompanyName,
                company.UserName!,
                company.PhoneNumber!,
                company.Email!);

            return new ApiResponse<object>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Company information retrieved successfully.",
                Data = companyResponse
            };
        }
        else if (account is Member member)
        {
            var memberResponse = new MemberResponse(currentUser.UserId,
                member.FullName,
                member.UserName!,
                member.PhoneNumber!,
                member.Email!,
                member.GenderId!.Value,
                member.Location);

            return new ApiResponse<object>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Member information retrieved successfully.",
                Data = memberResponse
            };
        }

        return Error.Unexpected(
            code: "User.UnknownType",
            description: "Unknown account type.");
    }
}