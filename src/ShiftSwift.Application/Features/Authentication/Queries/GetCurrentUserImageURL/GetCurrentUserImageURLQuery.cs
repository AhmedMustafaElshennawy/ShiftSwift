using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserImageURL;

public sealed record GetCurrentUserImageURLQuery(string UserId) : IRequest<ErrorOr<ApiResponse<string>>>;