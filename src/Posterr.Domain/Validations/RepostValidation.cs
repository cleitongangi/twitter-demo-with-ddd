using FluentValidation;
using Posterr.Domain.Entities;
using Posterr.Domain.Enum;
using Posterr.Domain.Interfaces.GlobalSettings;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Domain.Interfaces.Validations;

namespace Posterr.Domain.Validations
{
    public class RepostValidation : AbstractValidator<PostEntity>, IRepostValidation
    {
        private readonly IConfigSettings _configSettings;
        private readonly IPostRepository _postRepository;

        public RepostValidation(IConfigSettings configSettings, IPostRepository postRepository)
        {
            this._configSettings = configSettings;
            this._postRepository = postRepository;

            RuleFor(x => x.CreatedAt).GreaterThan(new DateTime(1900, 1, 1));
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.ParentId).GreaterThan(0);
            RuleFor(x => x.TypeId).Equal(2);

            RuleFor(x => x.UserId)
                .MustAsync((x, cancellation) => ReachedDailyPostLimitAsync(x)).WithMessage($"You have reached the daily limit of {_configSettings.DailyLimitPosts} posts.");

            // Check parent Post (in quote post)
            RuleFor(x => x).CustomAsync(async (x, context, cancellation) =>
                {
                    var postUserId = await _postRepository.GetPostUserIdAsync(x.ParentId ?? 0);
                    if (!postUserId.HasValue)
                    {
                        context.AddFailure("PostId", "'PostId' does not exist.");
                    }
                    else if (postUserId.Value == x.UserId)
                    {
                        context.AddFailure("PostId", "You cannot quote your post.");
                    }
                })
                .When(x => x.ParentId.HasValue);
        }

        private async Task<bool> ReachedDailyPostLimitAsync(long userId)
        {
            if (await _postRepository.PostCountSinceDateAsync(userId, DateTime.Now.Date) >= _configSettings.DailyLimitPosts)
            {
                return false;
            }
            return true;
        }
    }
}
