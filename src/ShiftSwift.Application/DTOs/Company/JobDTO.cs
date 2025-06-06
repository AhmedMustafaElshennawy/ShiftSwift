using ShiftSwift.Domain.Enums;

namespace ShiftSwift.Application.DTOs.Company;

public sealed record ListMyJobApplicaionsResponse(Guid JobId,
    string Title,
    string Description,
    string Location,
    DateTime PostedOn);