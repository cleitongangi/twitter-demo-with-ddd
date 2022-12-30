using Microsoft.Extensions.DependencyInjection;
using Posterr.Domain.Entities;
using Posterr.Domain.Enum;
using Posterr.Infra.Data.Context;
using Posterr.Infra.Data.Repositories.PosterrDb;
using Posterr.RestAPI.ApiInputs;
using Posterr.RestAPI.ApiResponses;
using Posterr.RestAPI.IntegrationTests.Utilities;
using System.Net;
using System.Net.Http.Json;

namespace Posterr.RestAPI.IntegrationTests.Controllers
{
    public class PostsControllerTests
    {
        [Fact]
        public async Task NewPost_ReturnSuccess()
        {
            // Arrange
            await using var application = new PosterrWebApplicationFactory();
                        
            // To ensure taht don't exceed the daily post limit
            await new DbRepository(application).UpdatePostsDateAsync();

            var input = new NewPostInput { Text = "IntegrationTest - New Post." };

            // Act
            var client = application.CreateClient();
            var result = await client.PostAsJsonAsync<NewPostInput>("/api/Posts/New", input);

            // Assert
            result.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task QuotePost_ReturnSuccess()
        {
            // Arrange
            await using var application = new PosterrWebApplicationFactory();            
            var dbRepository = new DbRepository(application);

            // To ensure taht don't exceed the daily post limit
            await dbRepository.UpdatePostsDateAsync();

            var input = new NewPostInput
            {
                Text = "IntegrationTest - Quote Post.",
                QuotePostId = await dbRepository.GetPostIdFromUser2Async()
            };

            // Act
            var client = application.CreateClient();
            var result = await client.PostAsJsonAsync<NewPostInput>("/api/Posts/New", input);

            // Assert
            result.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Repost_ReturnSuccess()
        {
            // Arrange
            await using var application = new PosterrWebApplicationFactory();
            var dbRepository = new DbRepository(application);

            // To ensure taht don't exceed the daily post limit
            await dbRepository.UpdatePostsDateAsync();

            var input = new RepostInput
            {
                PostId = await dbRepository.GetPostIdFromUser2Async()
            };

            // Act
            var client = application.CreateClient();
            var result = await client.PostAsJsonAsync<RepostInput>("/api/Posts/Repost", input);

            // Assert
            result.EnsureSuccessStatusCode();
        }
    }
}
