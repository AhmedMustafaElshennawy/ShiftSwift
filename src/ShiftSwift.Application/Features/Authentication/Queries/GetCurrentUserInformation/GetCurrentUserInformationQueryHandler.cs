using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.identity;
using System.Net;


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

        object response = account switch
        {
            Company company => company.Adapt<CurrentCompanyResponse>(),
            Member member => member.Adapt<CurrentMemberResponse>(),
            _ => null!
        };

        return new ApiResponse<object>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "User information retrieved successfully.",
            Data = response
        };
    }
}