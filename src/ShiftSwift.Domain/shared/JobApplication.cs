using ShiftSwift.Domain.identity;


namespace ShiftSwift.Domain.shared;

public class JobApplication
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public Job Job { get; set; }
    public string MemberId { get; set; }
    public Member Member { get; set; }
    public DateTime AppliedOn { get; set; } = DateTime.UtcNow;
    public string? Location { get; set; }
    public int Status { get; set; }
}