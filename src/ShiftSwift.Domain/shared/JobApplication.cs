using ShiftSwift.Domain.Enums;
using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.Shared;


namespace ShiftSwift.Domain.shared
{
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
        public ICollection<ApplicationAnswer> Answers { get; set; } = new List<ApplicationAnswer>();
    }
}
