using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.Resolver;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;


namespace ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserImageURL
{
    public sealed class GetCurrentUserImageURLQueryHndler : IRequestHandler<GetCurrentUserImageURLQuery, ErrorOr<ApiResponse<string>>>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Account> _accountRepository;
        private readonly AccountPictureResolver _accountPictureResolver;
        public GetCurrentUserImageURLQueryHndler(ICurrentUserProvider currentUserProvider, IBaseRepository<Account> accountRepository, AccountPictureResolver accountPictureResolver)
        {
            _currentUserProvider = currentUserProvider;
            _accountRepository = accountRepository;
            _accountPictureResolver = accountPictureResolver;
        }
        public async Task<ErrorOr<ApiResponse<string>>> Handle(GetCurrentUserImageURLQuery request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
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
            var account = await _accountRepository.Entites()
                .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken);

            if (account is null)
            {
                return Error.NotFound(
                    code: "User.NotFound",
                    description: "User not found.");
            }

            var imageUrl = _accountPictureResolver.Resolve(account);

            return new ApiResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "User profile picture URL retrieved successfully.",
                Data = imageUrl
            };
        }
    }
}