using ShiftSwift.Domain.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftSwift.Application.DTOs.Company
{
    public sealed record CompanyResponse(string CompanyId,
        string CompanyName,
        string UserName,
        string PhoneNumber,
        string Email,
        string Description);
}
