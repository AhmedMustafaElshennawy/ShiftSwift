using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;


namespace ShiftSwift.Application.Features.jobApplication.Query.GetLastWorkJobsForMember;

public sealed record GetMyLastWorkJobsQuery()
    : IRequest<ErrorOr<ApiResponse<IReadOnlyList<GetLastWorkJobResponse>>>>;

public sealed record GetLastWorkJobResponse(string CompanyId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? Overview,
    string? Field,
    DateTime? DateOfEstablish,
    string? Country,
    string? City,
    string? Area);