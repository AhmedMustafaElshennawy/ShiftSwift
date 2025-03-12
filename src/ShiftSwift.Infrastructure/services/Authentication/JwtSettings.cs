using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftSwift.Infrastructure.services.Authentication
{
    public class JwtSettings
    {
        public const string SectionName = "JWT";
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Key { get; set; } = null!;
        public double DurationInDays { get; set; }
    }
}
