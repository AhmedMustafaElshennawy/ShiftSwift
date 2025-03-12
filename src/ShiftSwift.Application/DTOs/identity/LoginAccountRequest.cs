using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftSwift.Application.DTOs.identity
{

    public sealed record LoginAccountRequest(
        string UserName,
        string Password);
}
