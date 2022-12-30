using FluentValidation.Results;

namespace Posterr.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<ValidationResult> FollowUserAsync(long userId, long targetUserId);
        Task<ValidationResult> UnfollowUserAsync(long userId, long targetUserId);
    }
}
