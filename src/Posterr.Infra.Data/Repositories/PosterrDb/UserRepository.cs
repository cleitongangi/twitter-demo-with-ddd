using Microsoft.EntityFrameworkCore;
using Posterr.Domain.Entities;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Posterr.Domain.DTOs;
using Microsoft.EntityFrameworkCore.Storage;

namespace Posterr.Infra.Data.Repositories.PosterrDb
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly IPosterrDbContext _db;

        public UserRepository(IPosterrDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<UserProfileDataDTO?> GetUserProfileDataAsync(long id, long authenticatedUserId)
        {
            var sql = @"select 
                         Id as UserId
                        ,u.Username
                        ,u.JoinedAt
                        ,u.FollowersCount
                        ,u.FollowingCount
                        ,u.PostsCount
                        ,(select top 1 1 from UserFollowing uf1 
	                        where uf1.UserId = @authenticatedUserId
	                        and uf1.TargetUserId = @id
	                        and Active = 1
                         ) as [Following]
                    from Users u                        
                    where u.id = @id";

            return await _db.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<UserProfileDataDTO>(sql, new { id, authenticatedUserId });
        }

        public async Task<bool> HasAsync(long userId)
        {
            return await _db.Users
                .AnyAsync(x => x.Id == userId);
        }

        public async Task<int> AddUserFollowingAsync(UserFollowingEntity entity)
        {
            var sql = @"insert into UserFollowing(
                             UserId
                            ,TargetUserId
                            ,CreatedAt
                            ,Active
                        )
                        SELECT 
                             @UserId
                            ,@TargetUserId
                            ,@CreatedAt
                            ,1
                        WHERE not exists (select 1 from UserFollowing 
	                        where UserId = @UserId
	                        and TargetUserId = @TargetUserId
	                        and Active = 1)"; // Add only if not exists

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, entity, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public async Task<int> UnfollowingUserAsync(UserFollowingEntity entity)
        {
            var sql = @"Update UserFollowing
                        Set 
                             Active = 0
                            ,RemovedAt = @RemovedAt                            
                        where UserId = @UserId
                            and TargetUserId = @TargetUserId
                            and Active = 1";

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, entity, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public async Task<int> IncrementFollowersCountAsync(long userId)
        {
            var sql = @"update Users
                    Set 
                         FollowersCount = FollowersCount + 1
                        ,MetricsUpdatedAt = getdate()
                    where Id = @userId";

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, new { userId }, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public async Task<int> DecrementFollowersCountAsync(long userId)
        {
            var sql = @"update Users
                    Set 
                         FollowersCount = FollowersCount - 1
                        ,MetricsUpdatedAt = getdate()
                    where Id = @userId";

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, new { userId }, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }
        
        public async Task<int> IncrementFollowingCountAsync(long userId)
        {
            var sql = @"update Users
                    Set 
                         FollowingCount = FollowingCount + 1
                        ,MetricsUpdatedAt = getdate()
                    where Id = @userId";

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, new { userId }, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public async Task<int> DecrementFollowingCountAsync(long userId)
        {
            var sql = @"update Users
                    Set 
                         FollowingCount = FollowingCount - 1
                        ,MetricsUpdatedAt = getdate()
                    where Id = @userId";

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, new { userId }, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public async Task<int> IncrementPostsCountAsync(long userId)
        {
            var sql = @"update Users
                    Set 
                         PostsCount = PostsCount + 1
                        ,MetricsUpdatedAt = getdate()
                    where Id = @userId";

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, new { userId }, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
