using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData
{
    public sealed record AddCompanyProfileDataCommand(string CompanyId,
       string CompanyName,
       string? Overview,
        string? Field,
        DateTime? DateOfEstablish,
        string? Country,
        string? City,
        string? Area) : IRequest<ErrorOr<ApiResponse<CompanyResponseInfo>>>;
}