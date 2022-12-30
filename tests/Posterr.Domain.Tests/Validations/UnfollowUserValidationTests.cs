using Moq;
using Posterr.Domain.Entities;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Domain.Validations;

namespace Posterr.Domain.Tests.Validations
{
    public class UnfollowUserValidationTests
    {
        [Fact]
        public async Task UnfollowUserValidation_ValidInput_ReturnValid()
        {
            // Mock
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.HasAsync(It.IsAny<long>())).Returns(Task.FromResult(true));

            // Arrange
            var entityToValidate = new UserFollowingEntity
            {
                UserId = 1,
                TargetUserId = 2,
                RemovedAt = DateTime.Now
            };

            // Act
            var validationResult = await new UnfollowUserValidation(userRepositoryMock.Object).ValidateAsync(entityToValidate);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task FollowUserValidation_UserIdDoesNotExist_ReturnInvalid()
        {
            // Mock
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.HasAsync(1)).Returns(Task.FromResult(false));
            userRepositoryMock.Setup(r => r.HasAsync(2)).Returns(Task.FromResult(true));

            // Arrange
            var entityToValidate = new UserFollowingEntity
            {
                UserId = 1,
                TargetUserId = 2,
                RemovedAt = DateTime.Now
            };

            // Act
            var validationResult = await new UnfollowUserValidation(userRepositoryMock.Object).ValidateAsync(entityToValidate);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'UserId' does not exist."));
            Assert.Equal(1, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task FollowUserValidation_UserIdAndTargetUserIdAreEquals_ReturnInvalid()
        {
            // Mock
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.HasAsync(It.IsAny<long>())).Returns(Task.FromResult(true));

            // Arrange
            var entityToValidate = new UserFollowingEntity
            {
                UserId = 1,
                TargetUserId = 1,
                RemovedAt = DateTime.Now
            };

            // Act
            var validationResult = await new UnfollowUserValidation(userRepositoryMock.Object).ValidateAsync(entityToValidate);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "You cannot unfollow yourself."));
            Assert.Equal(1, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task FollowUserValidation_InvalidInput_ReturnAllErrors()
        {
            // Mock
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.HasAsync(It.IsAny<long>())).Returns(Task.FromResult(false));
            
            // Arrange
            var entityToValidate = new UserFollowingEntity();

            // Act
            var validationResult = await new UnfollowUserValidation(userRepositoryMock.Object).ValidateAsync(entityToValidate);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'User Id' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Target User Id' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Removed At' must not be empty."));
            Assert.Equal(3, validationResult?.Errors.Count);
        }
    }
}