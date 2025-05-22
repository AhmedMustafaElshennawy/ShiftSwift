using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData
{
    public sealed class AddCompanyProfileDataCommandHandler : IRequestHandler<AddCompanyProfileDataCommand, ErrorOr<ApiResponse<CompanyResponseInfo>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        public AddCompanyProfileDataCommandHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<ErrorOr<ApiResponse<CompanyResponseInfo>>> Handle(AddCompanyProfileDataCommand request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
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
            var company = await _unitOfWork.Companies.Entites()
                .Where(M => M.Id == request.CompanyId)
                .SingleOrDefaultAsync(cancellationToken);

            if (company is null)
            {
                return Error.NotFound(
                    code: "User.NotFound",
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.CompanyId}");
            }

            company.CompanyName = request.CompanyName;
            company.Overview = request.Overview;
            company.Field = request.Field;
            company.DateOfEstablish = request.DateOfEstablish;
            company.Country = request.Country;
            company.City = request.City;
            company.Area = request.Area;
            await _unitOfWork.Companies.UpdateAsync(company);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var companyResponse = new CompanyResponseInfo(company.Id,
                company.CompanyName,
                company.UserName!,
                company.PhoneNumber!,
                company.Email!,
                company.Overview,
                company.Field,
                company.DateOfEstablish,
                company.Country,
                company.City,
                company.Area);

            return new ApiResponse<CompanyResponseInfo>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Company Data Added successfully",
                Data = companyResponse
            };
        }
    }
}
