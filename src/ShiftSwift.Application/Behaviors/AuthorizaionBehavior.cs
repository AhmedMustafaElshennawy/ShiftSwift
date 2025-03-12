//using ShiftSwift.Application.services.Authentication;


//namespace ShiftSwift.Application.Behaviors
//{
//    //    public sealed class AuthorizaionBehavior<TRequest, TResponse>(ICurrentUserProvider _currentUserProvider)
//    //        : IPipelineBehavior<TRequest, TResponse>
//    //        where TRequest : IRequest<TResponse>
//    //    {
//    //        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    //        {
//    //            var authorizationAttributes = request.GetType()
//    //           .GetCustomAttributes<AuthorizeAttribute>()
//    //           .ToList();

//    //            if (authorizationAttributes.Count == 0)
//    //            {
//    //                return await next();
//    //            }

//    //            var currentUser = _currentUserProvider.GetCurrentUser();

//    //            var requiredRoles = authorizationAttributes
//    //                .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? [])
//    //                .ToList();

//    //            if (requiredRoles.Except(currentUser.Roles).Any())
//    //            {
//    //                return (dynamic)Error.Unauthorized(description: "User is forbidden from taking this action");
//    //            }

//    //            return await next();
//    //        }
//    //    }


//using ErrorOr;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using ShiftSwift.Application.services.Authentication;
//using ShiftSwift.Shared.ApiBaseResponse;
//using System.Net;
//using System.Reflection;

//public sealed class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//    where TRequest : IRequest<TResponse>
//{
//    private readonly ICurrentUserProvider _currentUserProvider;
//    public AuthorizationBehavior(ICurrentUserProvider currentUserProvider) => _currentUserProvider = currentUserProvider;
//    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    {
//        // Get all Authorize attributes on the request
//        var authorizationAttributes = request.GetType()
//            .GetCustomAttributes<AuthorizeAttribute>()
//            .ToList();

//        // If there are no Authorize attributes, proceed to the next handler
//        if (authorizationAttributes.Count == 0)
//        {
//            return await next();
//        }

//        // Get the current user
//        var currentUserResult = await _currentUserProvider.GetCurrentUser();
//        if (currentUserResult.IsError)
//        {
//            // Handle the case where the current user cannot be retrieved
//            return HandleError(currentUserResult.Errors);
//        }

//        var currentUser = currentUserResult.Value;

//        // Get the required roles from the Authorize attributes
//        var requiredRoles = authorizationAttributes
//            .SelectMany(a => a.Roles?.Split(',') ?? Array.Empty<string>())
//            .ToList();

//        // Check if the current user has all the required roles
//        if (requiredRoles.Any() && requiredRoles.Except(currentUser.Roles).Any())
//        {
//            // Handle the case where the user does not have the required roles
//            return HandleError(Error.Unauthorized(description: "User is forbidden from taking this action"));
//        }

//        // Proceed to the next handler
//        return await next();
//    }

//    private TResponse HandleError(Error error)
//    {
//        // Handle the error based on the expected response type
//        if (typeof(TResponse) == typeof(ErrorOr<object>))
//        {
//            return (TResponse)(object)error;
//        }

//        if (typeof(TResponse) == typeof(ApiResponse<object>))
//        {
//            return (TResponse)(object)new ApiResponse<object>
//            {
//                IsSuccess = false,
//                StatusCode = HttpStatusCode.Forbidden,
//                Message = error.Description,
//                Data = null
//            };
//        }

//        throw new InvalidOperationException($"Unsupported response type: {typeof(TResponse)}");
//    }

//    private TResponse HandleError(List<Error> errors)
//    {
//        // Handle multiple errors
//        if (typeof(TResponse) == typeof(ErrorOr<object>))
//        {
//            return (TResponse)(object)errors;
//        }

//        if (typeof(TResponse) == typeof(ApiResponse<object>))
//        {
//            return (TResponse)(object)new ApiResponse<object>
//            {
//                IsSuccess = false,
//                StatusCode = HttpStatusCode.Forbidden,
//                Message = errors.FirstOrDefault().Description ?? "An error occurred.",
//                Data = null
//            };
//        }

//        throw new InvalidOperationException($"Unsupported response type: {typeof(TResponse)}");
//    }
//}