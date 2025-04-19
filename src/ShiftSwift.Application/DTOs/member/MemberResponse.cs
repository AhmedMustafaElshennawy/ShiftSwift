using ShiftSwift.Domain.Enums;

namespace ShiftSwift.Application.DTOs.member
{
    public sealed record MemberResponse(
       string memberId,
       string FullName,
       string UserName,
       string PhoneNumber,
       string Email,
       int GenderId,
       string Location);

    public sealed record GetApplicantsResponse(
       string MemberId,
       string FullName,
       string UserName,
       string PhoneNumber,
       string Email);

    public sealed record ApplyApplicantResponse(string MemberId,
       string FullName,
       string UserName,
       string PhoneNumber,
       string Email,
       int status);


    public sealed record MemberResponseInfo(
        string memberId,
        string FullName,
        string UserName,
        string PhoneNumber,
        string Email,
        int GenderId,
        string Location);
}
