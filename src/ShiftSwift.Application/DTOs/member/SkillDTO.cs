namespace ShiftSwift.Application.DTOs.member
{
    public sealed record SkillDTO(
        string Name);
    public sealed record UpdateSkillDTO(Guid SkillId,
        string Name);

    public sealed record AddSkillResponse(
        string MemberId,
        string Name);

    public sealed record SkillResponse(
        string MemberId,
        string Name);
}
