
namespace ShiftSwift.Application.DTOs.member
{
    public sealed record ExperienceDTO(string Title, 
        string CompanyName, 
        DateTime StartDate, 
        DateTime? EndDate,
        string? Description);

    public sealed record AddExperienceResponse(string MemberId,
        string Title,
        string CompanyName,
        DateTime StartDate,
        DateTime? EndDate,
        string? Description);

    public sealed record ExperienceResponse(string MemberId,
        string Title,
        string CompanyName,
        DateTime StartDate,
        DateTime? EndDate,
        string? Description);
}
