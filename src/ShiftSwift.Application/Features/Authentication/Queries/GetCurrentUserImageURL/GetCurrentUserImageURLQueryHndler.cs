using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using System.Net;
using ShiftSwift.Application.services.MediaService;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.Enums;


namespace ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserImageURL;

internal sealed class GetCurrentUserImageUrlQueryHndler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Account> accountRepository,
    IMediaService mediaService)
    : IRequestHandler<GetCurrentUserImageURLQuery, ErrorOr<ApiResponse<string>>>
{
    public async Task<ErrorOr<ApiResponse<string>>> Handle(GetCurrentUserImageURLQuery request,
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
        if (currentUser.UserId != request.UserId)
        {
            return Error.NotFound(
                code: "User.NotFound",
                description: "No User Found With This Id.");
        }

        var account = await accountRepository.Entites()
            .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken);

        if (account is null)
        {
            return Error.NotFound(
                code: "User.NotFound",
                description: "User not found.");
        }

        return new ApiResponse<string>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "User profile picture URL retrieved successfully.",
            Data = mediaService.GetUrl(account.ImageUrl,MediaTypes.Image)
        };
    }
}