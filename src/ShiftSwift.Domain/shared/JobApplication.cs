using ShiftSwift.Domain.identity;


namespace ShiftSwift.Domain.shared
{
    public class JobApplication 
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Job Job { get; set; }
        public string MemberId { get; set; }
        public Member Member { get; set; }
        public DateTime AppliedOn { get; set; }
        public bool Status { get; set; }  // accepted | Failed
    }
}
