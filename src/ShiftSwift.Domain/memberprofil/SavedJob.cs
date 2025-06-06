using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.shared;

namespace ShiftSwift.Domain.memberprofil;

public class SavedJob 
{
    public Guid Id { get; set; }
    public string MemberId { get; set; }
    public Member Member { get; set; }
    public Guid JobId { get; set; }
    public Job Job { get; set; }
    public DateTime SavedOn { get; set; }
}