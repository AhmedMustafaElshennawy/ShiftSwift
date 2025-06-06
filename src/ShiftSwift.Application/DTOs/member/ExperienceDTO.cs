namespace ShiftSwift.Application.DTOs.member;

public sealed record ExperienceDTO(
    string Title,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description);

public sealed record UpdateExperienceDTO(
    Guid ExperienceId,
    string Title,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description);

public sealed record AddExperienceResponse(
    Guid ExperienceId,
    string MemberId,
    string Title,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description);

public sealed record ExperienceResponse(
    string MemberId,
    string Title,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description);

public sealed record UpdateExperienceResponse(
    Guid ExperienceId,
    string MemberId,
    string Title,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description);