using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.identity;
using System.Net;
using Mapster;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetCompanyInfo;

internal sealed class GetCompanyInfoByIdQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Account> accountRepository) : IRequestHandler<GetCompanyInfoByIdQuery,
    ErrorOr<ApiResponse<CompanyInformationByIdResponse>>>
{
    public async Task<ErrorOr<ApiResponse<CompanyInformationByIdResponse>>> Handle(GetCompanyInfoByIdQuery request,
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


        if (account is not Company)
        {
            return Error.Forbidden(
                code: "User.Forbidden",
                description: "User is not Company Account Type.");
        }

        var companyResponse = account.Adapt<CompanyInformationByIdResponse>();

        return new ApiResponse<CompanyInformationByIdResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Company information retrieved successfully.",
            Data = companyResponse
        };
    }
}