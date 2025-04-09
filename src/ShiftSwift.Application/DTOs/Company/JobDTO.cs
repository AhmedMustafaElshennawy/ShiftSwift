namespace ShiftSwift.Application.DTOs.Company
{
    public sealed record JobDTO(string Title,
        string Description,
        string Location);

    public sealed record PostedJobResponse(string CompanyId,
        Guid JobId,
        string Title,
        string Description,
        string Location,
        DateTime PostedOn);

    public sealed record ListMyJobApplicaionsResponse(Guid JobId,
        string Title,
        string Description,
        string Location,
        DateTime PostedOn);
}
