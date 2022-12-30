using Posterr.Domain.DTOs;
using Posterr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddUserFollowingAsync(UserFollowingEntity entity);
        Task<int> DecrementFollowersCountAsync(long userId);
        Task<int> DecrementFollowingCountAsync(long userId);
        Task<UserProfileDataDTO?> GetUserProfileDataAsync(long id, long authenticatedUserId);
        Task<bool> HasAsync(long userId);
        Task<int> IncrementFollowersCountAsync(long userId);
        Task<int> IncrementFollowingCountAsync(long userId);
        Task<int> IncrementPostsCountAsync(long userId);
        Task<int> UnfollowingUserAsync(UserFollowingEntity entity);
    }
}
