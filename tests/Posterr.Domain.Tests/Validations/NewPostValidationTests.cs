using Moq;
using Posterr.Domain.Entities;
using Posterr.Domain.Interfaces.GlobalSettings;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.Domain.Tests.Validations
{
    public class NewPostValidationTests
    {
        [Fact]
        public async Task NewPostValidation_ValidInput_ReturnValid()
        {
            // Mock
            var postRepositoryMock = new Mock<IPostRepository>();
            postRepositoryMock.Setup(r => r.PostCountSinceDateAsync(It.IsAny<long>(), It.IsAny<DateTime>())).Returns(Task.FromResult(4));

            var configSettingsMock = new Mock<IConfigSettings>();
            configSettingsMock.Setup(r => r.DailyLimitPosts).Returns(5);

            // Arrange
            var postEntity = new PostEntity
            {
                CreatedAt = DateTime.Now,
                TypeId = 1,
                UserId = 1,
                Text = "New post test"
            };
            var newPostValidation = new NewPostValidation(configSettingsMock.Object, postRepositoryMock.Object);

            // Act
            var validationResult = await newPostValidation.ValidateAsync(postEntity);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task NewPostValidation_ReachedDailyLimit_ReturnInvalid()
        {
            // Mock
            var postRepositoryMock = new Mock<IPostRepository>();
            postRepositoryMock.Setup(r => r.PostCountSinceDateAsync(It.IsAny<long>(), It.IsAny<DateTime>())).Returns(Task.FromResult(6));

            var configSettingsMock = new Mock<IConfigSettings>();
            configSettingsMock.Setup(r => r.DailyLimitPosts).Returns(5);

            // Arrange
            var postEntity = new PostEntity
            {
                CreatedAt = DateTime.Now,
                TypeId = 1,
                UserId = 1,
                Text = "New post test"
            };
            var newPostValidation = new NewPostValidation(configSettingsMock.Object, postRepositoryMock.Object);

            // Act
            var validationResult = await newPostValidation.ValidateAsync(postEntity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "You have reached the daily limit of 5 posts."));
            Assert.Equal(1, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task NewPostValidation_InvalidInput_ReturnAllErrors()
        {
            // Mock
            var postRepositoryMock = new Mock<IPostRepository>();
            postRepositoryMock.Setup(r => r.PostCountSinceDateAsync(It.IsAny<long>(), It.IsAny<DateTime>())).Returns(Task.FromResult(6));

            var configSettingsMock = new Mock<IConfigSettings>();
            configSettingsMock.Setup(r => r.DailyLimitPosts).Returns(5);

            // Arrange
            var postEntity = new PostEntity();
            var newPostValidation = new NewPostValidation(configSettingsMock.Object, postRepositoryMock.Object);

            // Act
            var validationResult = await newPostValidation.ValidateAsync(postEntity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Text' must not be empty."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Created At' must be greater than '01/01/1900 00:00:00'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'User Id' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Type Id' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "You have reached the daily limit of 5 posts."));
            Assert.Equal(5, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task NewPostValidation_TextBigger_ReturnInvalid()
        {
            // Mock
            var postRepositoryMock = new Mock<IPostRepository>();
            postRepositoryMock.Setup(r => r.PostCountSinceDateAsync(It.IsAny<long>(), It.IsAny<DateTime>())).Returns(Task.FromResult(1));

            var configSettingsMock = new Mock<IConfigSettings>();
            configSettingsMock.Setup(r => r.DailyLimitPosts).Returns(5);

            // Arrange
            var postEntity = new PostEntity
            {
                Text = new string('x', 778),
                UserId = 1,
                TypeId = 1,
                CreatedAt = DateTime.Now
            };
            var newPostValidation = new NewPostValidation(configSettingsMock.Object, postRepositoryMock.Object);

            // Act
            var validationResult = await newPostValidation.ValidateAsync(postEntity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "The length of 'Text' must be 777 characters or fewer. You entered 778 characters."));
            Assert.Equal(1, validationResult?.Errors.Count);
        }
    }
}