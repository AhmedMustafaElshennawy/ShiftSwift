using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.Authentication.Commands.AddProfilePicture
{
    public sealed record AddProfilePictureCommand(IFormFile FormFile):IRequest<ErrorOr<ApiResponse<string>>>;
    
}
