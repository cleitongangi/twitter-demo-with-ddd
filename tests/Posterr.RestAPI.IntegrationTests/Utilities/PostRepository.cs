using Microsoft.Extensions.DependencyInjection;
using Posterr.Domain.Entities;
using Posterr.Domain.Enum;
using Posterr.Infra.Data.Context;

namespace Posterr.RestAPI.IntegrationTests.Utilities
{
    internal class DbRepository
    {
        private readonly PosterrWebApplicationFactory _application;

        public DbRepository(PosterrWebApplicationFactory application)
        {
            this._application = application;
        }

        internal async Task<long> GetPostIdFromUser2Async()
        {
            using var scope = _application.Services.CreateScope();
            using var posterrDbContext = scope.ServiceProvider.GetRequiredService<PosterrDbContext>();
            await posterrDbContext.Database.EnsureCreatedAsync();

            var postFromAnotherUser = posterrDbContext.Posts
                .FirstOrDefault(x =>
                        x.UserId.Equals(2)
                        && x.TypeId.Equals((int)PostTypeEnum.Post)
                    );

            if (postFromAnotherUser is null)
            {
                var newPost = new PostEntity
                {
                    CreatedAt = DateTime.Now,
                    UserId = 2,
                    TypeId = (int)PostTypeEnum.Post,
                    Text = "IntegrationTest - User2 Post."
                };
                await posterrDbContext.Posts.AddAsync(newPost);
                await posterrDbContext.SaveChangesAsync();

                return newPost.Id;
            }
            else
            {
                return postFromAnotherUser.Id;
            }
        }

        internal async Task UpdatePostsDateAsync()
        {
            using var scope = _application.Services.CreateScope();            
            using var posterrDbContext = scope.ServiceProvider.GetRequiredService<PosterrDbContext>();

            await posterrDbContext.Database.EnsureCreatedAsync();

            var posts = posterrDbContext.Posts
                .Where(x => x.UserId.Equals(1))
                .ToList();

            // Update
            posts.ForEach(x => x.CreatedAt = DateTime.Now.AddDays(-1));
            posterrDbContext.Posts.UpdateRange(posts);

            await posterrDbContext.SaveChangesAsync();
        }
    }
}
