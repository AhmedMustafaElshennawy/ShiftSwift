using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.education.Commands.DeleteEducation;

public sealed record DeleteEducationCommand(string MemberId) : IRequest<ErrorOr<ApiResponse<Deleted>>>;