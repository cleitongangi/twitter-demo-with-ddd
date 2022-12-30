using Posterr.Domain.Entities;
using Posterr.Domain.Enum;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Domain.Interfaces.Services;
using Posterr.Domain.Interfaces.Validations;
using FluentValidation.Results;
using Posterr.Domain.Interfaces.Data;
using Posterr.Domain.Core.Pagination;

namespace Posterr.Domain.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly INewPostValidation _newPostValidation;
        private readonly IRepostValidation _repostValidation;

        public PostService(IUnitOfWork unitOfWork,
                           IPostRepository postRepository,
                           IUserRepository userRepository,
                           INewPostValidation newPostValidation,
                           IRepostValidation repostValidation)
        {
            this._uow = unitOfWork;
            this._postRepository = postRepository;
            this._userRepository = userRepository;
            this._newPostValidation = newPostValidation;
            this._repostValidation = repostValidation;
        }

        public async Task<ValidationResult> AddNewPostAsync(long userId, string? text, long? quotePostId)
        {
            var postEntity = new PostEntity
            {
                CreatedAt = DateTime.Now,
                Text = text,
                TypeId = quotePostId.HasValue ? (int)PostTypeEnum.QuotePost : (int)PostTypeEnum.Post,
                UserId = userId,
                ParentId = quotePostId
            };

            var validationResult = await _newPostValidation.ValidateAsync(postEntity);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            try
            {
                await _uow.BeginTransactionAsync();

                await _postRepository.AddAsync(postEntity);                

                // Increment count metrics
                await _userRepository.IncrementPostsCountAsync(userId);

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

        public async Task<ValidationResult> RepostAsync(long userId, long postId)
        {
            var postEntity = new PostEntity
            {
                CreatedAt = DateTime.Now,                
                TypeId = (int)PostTypeEnum.Repost,
                UserId = userId,
                ParentId = postId
            };

            var validationResult = await _repostValidation.ValidateAsync(postEntity);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            try
            {
                await _uow.BeginTransactionAsync();
                await _postRepository.AddRepostAsync(postId, userId, DateTime.Now);

                // Increment count metrics
                await _userRepository.IncrementPostsCountAsync(userId);

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