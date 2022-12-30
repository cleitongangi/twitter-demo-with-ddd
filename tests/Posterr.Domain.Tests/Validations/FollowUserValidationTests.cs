using Moq;
using Posterr.Domain.Entities;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Domain.Validations;

namespace Posterr.Domain.Tests.Validations
{
    public class FollowUserValidationTests
    {
        [Fact]
        public async Task FollowUserValidation_ValidInput_ReturnValid()
        {
            // Mock
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.HasAsync(It.IsAny<long>())).Returns(Task.FromResult(true));

            // Arrange
            var entityToValidate = new UserFollowingEntity
            {
                UserId = 1,
                TargetUserId = 2,
                CreatedAt = DateTime.Now,
                Active = true
            };

            // Act
            var validationResult = await new FollowUserValidation(userRepositoryMock.Object).ValidateAsync(entityToValidate);

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
                CreatedAt = DateTime.Now,
                Active = true
            };

            // Act
            var validationResult = await new FollowUserValidation(userRepositoryMock.Object).ValidateAsync(entityToValidate);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'UserId' does not exist."));
            Assert.Equal(1, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task FollowUserValidation_TargetUserIdDoesNotExist_ReturnInvalid()
        {
            // Mock
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.HasAsync(1)).Returns(Task.FromResult(true));
            userRepositoryMock.Setup(r => r.HasAsync(2)).Returns(Task.FromResult(false));

            // Arrange
            var entityToValidate = new UserFollowingEntity
            {
                UserId = 1,
                TargetUserId = 2,
                CreatedAt = DateTime.Now,
                Active = true
            };

            // Act
            var validationResult = await new FollowUserValidation(userRepositoryMock.Object).ValidateAsync(entityToValidate);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'TargetUserId' does not exist."));
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
                CreatedAt = DateTime.Now,
                Active = true
            };

            // Act
            var validationResult = await new FollowUserValidation(userRepositoryMock.Object).ValidateAsync(entityToValidate);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "You cannot follow yourself."));
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
            var validationResult = await new FollowUserValidation(userRepositoryMock.Object).ValidateAsync(entityToValidate);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'User Id' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Target User Id' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Created At' must be greater than '01/01/1900 00:00:00'."));
            Assert.Equal(3, validationResult?.Errors.Count);
        }
    }
}