using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserInformation
{
    public sealed record GetCurrentUserInformationQuery():IRequest<ErrorOr<ApiResponse<object>>>;
    
}
