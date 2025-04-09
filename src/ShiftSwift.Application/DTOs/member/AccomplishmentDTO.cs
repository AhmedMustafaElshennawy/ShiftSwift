namespace ShiftSwift.Application.DTOs.member
{
    public sealed record AccomplishmentDTO(
        string Title,
        string? Description,
        DateTime? DateAchieved);

    public sealed record AccomplishmentResponse(
        Guid Id,
        string MemberId,
        string Title,
        string? Description,
        DateTime? DateAchieved);
}

