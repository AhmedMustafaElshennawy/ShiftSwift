namespace ShiftSwift.Application.DTOs.Company;

public sealed record CompanyResponse(string CompanyId,
    string CompanyName,
    string UserName,
    string PhoneNumber,
    string Email);

public sealed record CompanyResponseInfo(string CompanyId,
    string FirstName,
    string LastName,
    string CompanyName,
    string UserName,
    string PhoneNumber,
    string Email,
    string? Overview,
    string? Field,
    DateTime? DateOfEstablish,
    string? Country,
    string? City,
    string? Area);