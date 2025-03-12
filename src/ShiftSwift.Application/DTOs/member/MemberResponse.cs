namespace ShiftSwift.Application.DTOs.member
{
    public record MemberResponse(
       string memberId,
       string FullName,
       string UserName,
       string PhoneNumber,
       string Email);

    public record GetApplicantsResponse(
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
       bool status);
}
