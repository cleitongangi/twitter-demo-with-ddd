using Posterr.Domain.Core.Pagination;
using Posterr.Domain.Entities;

namespace Posterr.Domain.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task AddAsync(PostEntity entity);
        Task<int> AddRepostAsync(long parentPostId, long userId, DateTime createdAt);
        Task<PagedResult<PostEntity>> GetAllPostsAsync(int page, int pageSize);
        Task<PagedResult<PostEntity>> GetFollowingPostsAsync(int page, int pageSize, long userId);
        Task<PagedResult<PostEntity>> GetPostsByUserIdAsync(int page, int pageSize, long userId);
        Task<long?> GetPostUserIdAsync(long postId);
        Task<int> PostCountSinceDateAsync(long userId, DateTime sinceDate);
        Task<IEnumerable<PostEntity>> SearchAsync(string textToSearch);
    }
}
