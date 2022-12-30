using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Posterr.Domain.Entities
{
    public class PostTypeEntity
    {
        public int Id { get; set; } // Id (Primary key)
        public string TypeDescription { get; set; } = null!; // TypeDescription (length: 6)

        // Reverse navigation

        /// <summary>
        /// Child Posts where [Posts].[TypeId] point to this entity (FK_Posts_PostTypes)
        /// </summary>
        public virtual ICollection<PostEntity> Posts { get; set; } // Posts.FK_Posts_PostTypes

        public PostTypeEntity()
        {
            Posts = new List<PostEntity>();
        }
    }
}