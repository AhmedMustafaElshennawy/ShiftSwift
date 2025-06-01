using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Commands.AddProfilePicture;

public sealed record AddProfilePictureCommand(IFormFile Image):IRequest<ErrorOr<ApiResponse<string>>>;