using FluentValidation;
using Posterr.Domain.Entities;

namespace Posterr.Domain.Interfaces.Validations
{
    public interface IUnfollowUserValidation : IValidator<UserFollowingEntity>
    {
    }
}
