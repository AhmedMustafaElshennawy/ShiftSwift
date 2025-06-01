using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;


namespace ShiftSwift.Application.Features.Authentication.Commands.LogOut
{
    public sealed record LogoutCommand():IRequest<ErrorOr<ApiResponse<string>>>;
    
}
