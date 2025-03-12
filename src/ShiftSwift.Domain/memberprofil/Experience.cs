using ShiftSwift.Domain.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftSwift.Domain.models.memberprofil
{
    public class Experience
    {
        public Guid Id { get; set; }
        public string MemberId { get; set; }
        public Member Member { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
    }
}
