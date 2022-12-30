using Posterr.Domain.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.Domain.DTOs
{
    public class UserProfileDataDTO
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public DateTime JoinedAt { get; set; }
        public string JoinedAtFormatted => JoinedAt.ToString("MMMM dd, yyyy").ToUpperFirstLetter();
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int PostsCount { get; set; }
        public bool Following { get; set; }

        public UserProfileDataDTO(long userId, string username, DateTime joinedAt, int followersCount, int followingCount, int postsCount, bool following)
        {
            UserId = userId;
            Username = username;
            JoinedAt = joinedAt;
            FollowersCount = followersCount;
            FollowingCount = followingCount;
            PostsCount = postsCount;
            Following = following;
        }

        public UserProfileDataDTO()
            : this(userId: 0, username: string.Empty, joinedAt: DateTime.MinValue, followersCount: 0, followingCount: 0, postsCount: 0, following: false)
        {
        }
    }
}
