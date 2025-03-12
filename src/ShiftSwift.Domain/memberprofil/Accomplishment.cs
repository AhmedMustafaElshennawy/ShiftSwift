using ShiftSwift.Domain.identity;

namespace ShiftSwift.Domain.models.memberprofil
{
    public class Accomplishment
    {
        public Guid Id { get; set; }
        public string MemberId { get; set; }
        public Member Member { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DateAchieved { get; set; }
    }
}
