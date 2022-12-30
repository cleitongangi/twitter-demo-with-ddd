using FluentValidation.Results;
using Posterr.Domain.Entities;
using Posterr.Domain.Interfaces.Data;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Domain.Interfaces.Services;
using Posterr.Domain.Interfaces.Validations;

namespace Posterr.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IFollowUserValidation _followUserValidation;
        private readonly IUnfollowUserValidation _unfollowUserValidation;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository, IFollowUserValidation followUserValidation, IUnfollowUserValidation unfollowUserValidation)
        {
            this._uow = unitOfWork;
            this._userRepository = userRepository;
            this._followUserValidation = followUserValidation;
            this._unfollowUserValidation = unfollowUserValidation;
        }

        public async Task<ValidationResult> FollowUserAsync(long userId, long targetUserId)
        {
            var entity = new UserFollowingEntity
            {
                UserId = userId,
                TargetUserId = targetUserId,
                CreatedAt = DateTime.Now,
                Active = true
            };

            var validationResult = await _followUserValidation.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            try
            {
                await _uow.BeginTransactionAsync();

                var rowsAffected = await _userRepository.AddUserFollowingAsync(entity);
                if (rowsAffected == 0)
                { // If affected no rows, is because the users already follow the target user.
                    validationResult.Errors.Add(new ValidationFailure("", "You already follow this user."));
                    await _uow.RollbackAsync();
                    return validationResult;
                }

                // Increment count metrics
                await _userRepository.IncrementFollowingCountAsync(userId);
                await _userRepository.IncrementFollowersCountAsync(targetUserId);                

                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();

                return validationResult;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<ValidationResult> UnfollowUserAsync(long userId, long targetUserId)
        {
            var entity = new UserFollowingEntity
            {
                UserId = userId,
                TargetUserId = targetUserId,
                RemovedAt = DateTime.Now
            };

            var validationResult = await _unfollowUserValidation.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            try
            {
                await _uow.BeginTransactionAsync();

                var rowsAffected = await _userRepository.UnfollowingUserAsync(entity);
                if (rowsAffected == 0)
                { // If affected no rows, is because the user did not follow the target user.
                    validationResult.Errors.Add(new ValidationFailure("", "You did not follow this user."));
                    await _uow.RollbackAsync();
                    return validationResult;
                }

                // Decrement count metrics
                await _userRepository.DecrementFollowingCountAsync(userId);
                await _userRepository.DecrementFollowersCountAsync(targetUserId);

                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();

                return validationResult;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
