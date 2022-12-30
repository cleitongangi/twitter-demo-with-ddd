using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Posterr.Domain.Entities
{
    public class UserEntity
    {
        public long Id { get; set; } // Id (Primary key)
        public string Username { get; set; } = null!; // Username (length: 14)
        public DateTime JoinedAt { get; set; } // JoinedAt
        public int FollowersCount { get; set; } // FollowersCount
        public int FollowingCount { get; set; } // FollowingCount
        public int PostsCount { get; set; } // PostsCount
        public DateTime MetricsUpdatedAt { get; set; } // MetricsUpdatedAt

        // Reverse navigation

        /// <summary>
        /// Child Posts where [Posts].[UserId] point to this entity (FK_Posts_Users)
        /// </summary>
        public virtual ICollection<PostEntity> Posts { get; set; } // Posts.FK_Posts_Users
        
        public UserEntity()
        {
            Posts = new List<PostEntity>();            
        }
    }
}