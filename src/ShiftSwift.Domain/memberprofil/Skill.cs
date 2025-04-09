using ShiftSwift.Domain.identity;

namespace ShiftSwift.Domain.models.memberprofil
{
    public class Skill
    {
        public Guid Id { get; set; }
        public string MemberId { get; set; }
        public Member Member { get; set; }
        public string Name { get; set; }
    }
}
