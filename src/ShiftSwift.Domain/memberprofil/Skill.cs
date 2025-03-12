using ShiftSwift.Domain.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
