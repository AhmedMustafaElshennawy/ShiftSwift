using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;


namespace ShiftSwift.Application.Features.Authentication.Commands.LogOut
{
    public sealed record LogoutCommand():IRequest<ErrorOr<ApiResponse<string>>>;
    
}
