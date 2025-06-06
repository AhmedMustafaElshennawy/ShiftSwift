using FluentValidation;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetMemberInfo;

public sealed class GetMemberInfoByIdValidatior : AbstractValidator<GetMemberInfoById>
{
    public GetMemberInfoByIdValidatior()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("The UserId Required.");
    }
}