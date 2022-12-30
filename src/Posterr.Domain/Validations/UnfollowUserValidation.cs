﻿using FluentValidation;
using Posterr.Domain.Entities;
using Posterr.Domain.Interfaces.Repositories;
using Posterr.Domain.Interfaces.Validations;

namespace Posterr.Domain.Validations
{
    public class UnfollowUserValidation : AbstractValidator<UserFollowingEntity>, IUnfollowUserValidation
    {
        public UnfollowUserValidation(IUserRepository userRepository)
        {
            RuleFor(x => x.UserId).GreaterThan(0);

            RuleFor(x => x.TargetUserId).GreaterThan(0);

            RuleFor(x => x.RemovedAt)
                .GreaterThan(new DateTime(1900, 1, 1))
                .NotNull();

            RuleFor(x => x.UserId)
                .NotEqual(x => x.TargetUserId)
                .WithMessage("You cannot unfollow yourself.")
                .When(x => x.UserId != 0);

            RuleFor(x => x.UserId)
                .MustAsync(async (x, cancellation) => await userRepository.HasAsync(x))
                .WithMessage("'UserId' does not exist.")
                .When(x => x.UserId != 0);
        }
    }
}
