using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Posterr.Domain.Entities
{
    public class PostEntity
    {
        public long Id { get; set; } // Id (Primary key)
        public string? Text { get; set; } // Text (length: 777)
        public DateTime CreatedAt { get; set; } // CreatedAt
        public long UserId { get; set; } // UserId        
        public long? ParentId { get; set; } // ParentId
        public int TypeId { get; set; } // TypeId

        // Reverse navigation

        /// <summary>
        /// Child Posts where [Posts].[ParentId] point to this entity (FK_Posts_PostParent)
        /// </summary>
        public virtual ICollection<PostEntity> Posts { get; set; } // Posts.FK_Posts_PostParent

        // Foreign keys

        /// <summary>
        /// Parent Post pointed by [Posts].([ParentId]) (FK_Posts_PostParent)
        /// </summary>
        public virtual PostEntity? Parent { get; set; } // FK_Posts_PostParent

        /// <summary>
        /// Parent PostType pointed by [Posts].([TypeId]) (FK_Posts_PostTypes)
        /// </summary>
        public virtual PostTypeEntity? PostType { get; set; } // FK_Posts_PostTypes

        /// <summary>
        /// Parent User pointed by [Posts].([UserId]) (FK_Posts_Users)
        /// </summary>
        public virtual UserEntity? User { get; set; } // FK_Posts_Users

        public PostEntity()
        {
            Posts = new List<PostEntity>();
        }
    }
}