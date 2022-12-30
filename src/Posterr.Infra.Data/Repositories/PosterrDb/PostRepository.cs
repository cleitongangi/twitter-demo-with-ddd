using Microsoft.EntityFrameworkCore;
using Posterr.Domain.Entities;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Infra.Data.Context;
using Posterr.Domain.Core.Extensions;
using Dapper;
using Posterr.Domain.Enum;
using Microsoft.EntityFrameworkCore.Storage;
using Posterr.Domain.Core.Pagination;
using System.Text;

namespace Posterr.Infra.Data.Repositories.PosterrDb
{
    public class PostRepository : IPostRepository, IDisposable
    {
        private readonly IPosterrDbContext _db;

        public PostRepository(IPosterrDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<int> PostCountSinceDateAsync(long userId, DateTime sinceDate)
        {
            var sql = @"select count(*) from Posts
                    where UserId = @userId
                     and CreatedAt >= @sinceDate";

            return await _db.Database.GetDbConnection()
                .QueryFirstAsync<int>(sql, new { userId, sinceDate });
        }

        public async Task AddAsync(PostEntity entity)
        {
            await _db.Posts.AddAsync(entity);
        }

        public async Task<int> AddRepostAsync(long parentPostId, long userId, DateTime createdAt)
        {
            var sql = @"insert into Posts(                         
                         [Text]
                        ,CreatedAt
                        ,UserId                        
                        ,ParentId
                        ,TypeId
                    )
                    select
                         [Text]
                        ,@createdAt
                        ,@userId                        
                        ,@parentPostId
                        ,@typeId
                    from Posts
                    where Id = @parentPostId";

            var parameters = new
            {
                parentPostId,
                userId,
                typeId = (int)PostTypeEnum.Repost,
                createdAt
            };
            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, parameters, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public async Task<long?> GetPostUserIdAsync(long postId)
        {
            var sql = @"select UserId from Posts
                    where Id = @postId";

            return await _db.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<long?>(sql, new { postId });
        }

        public async Task<PagedResult<PostEntity>> GetAllPostsAsync(int page, int pageSize)
        {
            return await GetPostsAsync(page, pageSize, null, false);
        }

        public async Task<PagedResult<PostEntity>> GetPostsByUserIdAsync(int page, int pageSize, long userId)
        {
            return await GetPostsAsync(page, pageSize, userId, false);
        }

        public async Task<PagedResult<PostEntity>> GetFollowingPostsAsync(int page, int pageSize, long userId)
        {
            return await GetPostsAsync(page, pageSize, userId, true);
        }

        private async Task<PagedResult<PostEntity>> GetPostsAsync(int page, int pageSize, long? userId, bool onlyFollowingPosts = false)
        {
            var sql = new StringBuilder();
            sql.Append(@"SELECT 
	                     post.Id                        
	                    ,post.TypeId
	                    ,post.Text
	                    ,post.UserId
	                    ,post.CreatedAt	                    
	                    ,post.ParentId
                        ,t.TypeDescription
	                    ,parent.Text
	                    ,parent.UserId
	                    ,parent.CreatedAt
                    FROM Posts post
                        inner join PostTypes t on t.Id = post.TypeId
	                    LEFT JOIN Posts parent ON parent.Id = post.ParentId
                    WHERE 0 = 0");
            if (onlyFollowingPosts)
            { // Get only posts from followings
                if (!userId.HasValue)
                {
                    throw new ArgumentNullException("userId", "Param [userId] must be filled when [onlyFollowingPosts] = true");
                }
                sql.Append(@"
                        AND EXISTS(SELECT 1 FROM UserFollowing uf1 WHERE uf1.UserId = @userId
                                            AND uf1.TargetUserId = post.UserId
                                            AND uf1.Active = 1)");
            }
            else if (userId.HasValue)
            { // Get only posts posted by userId
                sql.Append(@"
                        AND post.UserId = @userId");

            }
            sql.Append(@"
                    ORDER BY post.CreatedAt desc
                    OFFSET (@page-1)*@pageSize ROWS
                    FETCH NEXT @pageSize ROWS ONLY");


            var data = await _db.Database.GetDbConnection()
                .QueryAsync<PostEntity, PostTypeEntity, PostEntity, PostEntity>(sql.ToString(),
                    (post, postType, parent) =>
                    {
                        post.PostType = postType;
                        if (parent != null)
                        {
                            post.Parent = parent;
                            post.Parent.Id = post.ParentId ?? 0;
                        }
                        return post;
                    },
                    splitOn: "TypeDescription, Text",
                    param: new { page, pageSize, userId }
                );

            return data.ConvertToPagedResult(page, pageSize);
        }

        public async Task<IEnumerable<PostEntity>> SearchAsync(string textToSearch)
        {
            var sql = @"SELECT 
	                     post.Id                        
	                    ,post.TypeId
	                    ,post.[Text]
	                    ,post.UserId
	                    ,post.CreatedAt	                    
	                    ,post.ParentId
                        ,t.TypeDescription
	                    ,parent.Text
	                    ,parent.UserId
	                    ,parent.CreatedAt
                    FROM Posts post
                        inner join PostTypes t on t.Id = post.TypeId
	                    LEFT JOIN Posts parent ON parent.Id = post.ParentId
                    WHERE 
                        post.TypeId in (1,3)
                        AND CONTAINS(post.[Text],@textToSearch)
                    ORDER BY post.CreatedAt desc";

            return await _db.Database.GetDbConnection()
                .QueryAsync<PostEntity, PostTypeEntity, PostEntity, PostEntity>(sql.ToString(),
                    (post, postType, parent) =>
                    {
                        post.PostType = postType;
                        if (parent != null)
                        {
                            post.Parent = parent;
                            post.Parent.Id = post.ParentId ?? 0;
                        }
                        return post;
                    },
                    splitOn: "TypeDescription, Text",
                    param: new { textToSearch }
                );
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}