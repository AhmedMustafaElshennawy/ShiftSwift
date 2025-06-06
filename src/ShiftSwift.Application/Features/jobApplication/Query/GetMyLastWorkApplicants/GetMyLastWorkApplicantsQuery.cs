using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetMyLastWorkApplicants;

public sealed record GetMyLastWorkApplicantsQuery(string CompanyId)
    : IRequest<ErrorOr<ApiResponse<IReadOnlyList<GetMyLastWorkApplicantsResponse>>>>;

public sealed record GetMyLastWorkApplicantsResponse(
    string MemberId,
    string FullName,
    string UserName,
    string PhoneNumber,
    string Email);