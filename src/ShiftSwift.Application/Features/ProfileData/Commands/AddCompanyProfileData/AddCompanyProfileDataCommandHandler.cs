using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using Mapster;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData;

public sealed class AddCompanyProfileDataCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<AddCompanyProfileDataCommand, ErrorOr<ApiResponse<AddOrUpdateCompanyProfileInformationResponse>>>
{
    public async Task<ErrorOr<ApiResponse<AddOrUpdateCompanyProfileInformationResponse>>> Handle(
        AddCompanyProfileDataCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = await currentUserProvider.GetCurrentUser();
        if (currentUserResult.IsError)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: currentUserResult.Errors.FirstOrDefault().Description ?? "User is not authenticated.");
        }

        var currentUser = currentUserResult.Value;
        if (!currentUser.Roles.Contains("Company"))
        {
            return Error.Forbidden(
                code: "User.Forbidden",
                description: "Access denied. Only companies can add profile data.");
        }

        if (currentUser.UserId != request.CompanyId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: $"Access denied. The MemberId You Entered Is Wrong {request.CompanyId}");
        }

        var company = await unitOfWork.Companies.Entites()
            .Where(m => m.Id == request.CompanyId)
            .SingleOrDefaultAsync(cancellationToken);

        if (company is null)
        {
            return Error.NotFound(
                code: "User.NotFound",
                description: $"Access denied. The MemberId You Entered Is Wrong {request.CompanyId}");
        }

        company.FirstName = request.FirstName;
        company.LastName = request.LastName;
        company.Overview = request.Overview;
        company.Field = request.Field;
        company.DateOfEstablish = request.DateOfEstablish;
        company.Country = request.Country;
        company.City = request.City;
        company.Area = request.Area;
        company.PhoneNumber = request.PhoneNumber;
        await unitOfWork.Companies.UpdateAsync(company);
        await unitOfWork.CompleteAsync(cancellationToken);

        var companyResponse = company.Adapt<AddOrUpdateCompanyProfileInformationResponse>();

        return new ApiResponse<AddOrUpdateCompanyProfileInformationResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Company Data Added successfully",
            Data = companyResponse
        };
    }
}