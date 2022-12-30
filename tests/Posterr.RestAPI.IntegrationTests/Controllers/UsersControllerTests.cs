using Posterr.Domain.Core.Pagination;
using Posterr.RestAPI.ApiInputs;
using Posterr.RestAPI.ApiResponses;
using System.Net.Http.Json;

namespace Posterr.RestAPI.IntegrationTests.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task GetUser_ReturnSuccessAndCorrectContent()
        {
            // Arrange
            await using var application = new PosterrWebApplicationFactory();
            var client = application.CreateClient();

            // Act
            var user = await client.GetFromJsonAsync<GetUserResponse>("/api/Users/1");

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user?.UserId);
        }

        [Fact]
        public async Task Following_ReturnSuccess()
        {
            // Arrange
            await using var application = new PosterrWebApplicationFactory();
            var client = application.CreateClient();
            
            var input = new FollowingUserInput { TargetUserId = 2 };

            // Call "DELETE /api/Users/Following" before to ensure that don't follow the user
            await client.DeleteAsync("/api/Users/Following/2");

            // Act
            var result = await client.PostAsJsonAsync<FollowingUserInput>("/api/Users/Following", input);

            // Assert
            result.EnsureSuccessStatusCode();            
        }

        [Fact]
        public async Task Unfollowing_ReturnSuccess()
        {
            // Arrange
            await using var application = new PosterrWebApplicationFactory();
            var client = application.CreateClient();

            var input = new FollowingUserInput { TargetUserId = 2 };

            // Call "POST /api/Users/Following" before to ensure that you follow the user
            await client.PostAsJsonAsync<FollowingUserInput>("/api/Users/Following", input);            
            
            // Act
            var result = await client.DeleteAsync("/api/Users/Following/2");

            // Assert
            result.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UserPosts_ReturnSuccessAndCorrectContent()
        {
            // Arrange
            await using var application = new PosterrWebApplicationFactory();
            var client = application.CreateClient();

            // Act
            var posts = await client.GetFromJsonAsync<PagedResult<GetFeedPostsResponse>>("/api/Users/1/Posts");

            // Assert            
            Assert.NotNull(posts);
            Assert.Equal(1, posts?.CurrentPage);
            Assert.Equal(5, posts?.PageSize);
        }
    }
}
