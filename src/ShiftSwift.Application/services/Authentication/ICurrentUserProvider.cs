

using ErrorOr;

namespace ShiftSwift.Application.services.Authentication
{
    public interface ICurrentUserProvider
    {
        public Task<ErrorOr<CurrentUser>> GetCurrentUser();
    }
}
