﻿namespace ShiftSwift.Application.DTOs.member
{
    public sealed record JobApplicationDTO(Guid JobId,
        string MemberId);

    public sealed record JobApplicationResponse(Guid Id,
        Guid JobId, 
        string MemberId,
        DateTime AppliedOn);

}
