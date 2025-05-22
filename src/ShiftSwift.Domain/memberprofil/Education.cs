using ShiftSwift.Domain.identity;

namespace ShiftSwift.Domain.memberprofil;

public class Education
{
    public Guid Id { get; set; }
    public string MemberId { get; set; }
    public Member Member { get; set; }
    public required string Level { get; set; }
    public required string Faculty { get; set; }
    public required string UniversityName { get; set; }
}