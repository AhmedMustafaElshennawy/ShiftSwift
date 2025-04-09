namespace ShiftSwift.Application.DTOs.Company
{
    public sealed record CompanyResponse(string CompanyId,
        string CompanyName,
        string UserName,
        string PhoneNumber,
        string Email,
        string Description);
}
