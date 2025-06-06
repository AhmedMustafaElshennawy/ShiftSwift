using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Commands.DeleteCurrentUserImage;

public sealed record DeleteCurrentUserImageUrlCommand(string UserId):IRequest<ErrorOr<ApiResponse<Unit>>>;