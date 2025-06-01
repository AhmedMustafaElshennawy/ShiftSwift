namespace ShiftSwift.Application.DTOs.Company;

public record CompanyProfileDataDTO(
    string FirstName,
    string LAstName,
    string? Overview,
    string? Field,
    DateTime? DateOfEstablish,
    string? Country,
    string? City,
    string? Area);