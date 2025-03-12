namespace ShiftSwift.Application.DTOs.member
{
    public sealed record SavedJobsResponse(
        Guid Id,
        Guid JobId,
        string JobTitle,
        string CompanyName,
        DateTime SavedOn);
}
