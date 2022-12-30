using Posterr.Domain.Core.Pagination;
using Posterr.RestAPI.ApiResponses;
using System.Net.Http.Json;

namespace Posterr.RestAPI.IntegrationTests.Controllers
{
    public class FeedControllerTests
    {
        [Fact]
        public async Task AllPosts_ReturnSuccessAndCorrectContent()
        {
            // Arrange
            await using var application = new PosterrWebApplicationFactory();
            var client = application.CreateClient();

            // Act
            var posts = await client.GetFromJsonAsync<PagedResult<GetFeedPostsResponse>>("/api/Feed/Posts");

            // Assert            
            Assert.NotNull(posts);
            Assert.Equal(1, posts?.CurrentPage);
            Assert.Equal(10, posts?.PageSize);
        }

        [Fact]
        public async Task FollowingPost_ReturnSuccessAndCorrectContent()
        {
            // Arrange
            await using var application = new PosterrWebApplicationFactory();
            var client = application.CreateClient();

            // Act
            var posts = await client.GetFromJsonAsync<PagedResult<GetFeedPostsResponse>>("/api/Feed/Posts/Folowing");

            // Assert            
            Assert.NotNull(posts);
            Assert.Equal(1, posts?.CurrentPage);
            Assert.Equal(10, posts?.PageSize);
        }
    }
}
