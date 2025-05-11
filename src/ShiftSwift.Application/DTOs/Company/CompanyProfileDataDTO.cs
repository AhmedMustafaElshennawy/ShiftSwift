namespace ShiftSwift.Application.DTOs.Company
{
    public record CompanyProfileDataDTO(
        string CompanyName,
        string? Overview,
        string? Field,
        DateTime? DateOfEstablish,
        string? Country,
        string? City,
        string? Area);

}
