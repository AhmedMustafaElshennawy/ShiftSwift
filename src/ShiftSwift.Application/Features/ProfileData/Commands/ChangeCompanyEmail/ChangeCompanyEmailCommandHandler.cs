using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using ShiftSwift.Application.Features.Authentication.Commands.RegisterCompany;
using ShiftSwift.Domain.ApiResponse;


namespace ShiftSwift.Application.Features.ProfileData.Commands.ChangeCompanyEmail;

public sealed class ChangeCompanyEmailCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<ChangeCompanyEmailCommand, ErrorOr<ApiResponse<CompanyResponse>>>
{
    public async Task<ErrorOr<ApiResponse<CompanyResponse>>> Handle(ChangeCompanyEmailCommand request, CancellationToken cancellationToken)
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
                description: "Access denied. Only members can add education.");
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
        company.Email = request.Email;
        await unitOfWork.Companies.UpdateAsync(company);
        await unitOfWork.CompleteAsync(cancellationToken);

        var companyResponse = new CompanyResponse(company.Id,
            company.UserName!,
            company.PhoneNumber!,
            company.Email!);

        return new ApiResponse<CompanyResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Company Data Added successfully",
            Data = companyResponse
        };
    }
}