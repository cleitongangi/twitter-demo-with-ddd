using FluentValidation.Results;
using Posterr.Domain.Core.Pagination;
using Posterr.Domain.Entities;

namespace Posterr.Domain.Interfaces.Services
{
    public interface IPostService
    {
        Task<ValidationResult> AddNewPostAsync(long userId, string? text, long? quotePostId);        
        Task<ValidationResult> RepostAsync(long userId, long postId);
    }
}
