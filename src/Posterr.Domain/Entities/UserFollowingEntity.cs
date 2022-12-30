namespace Posterr.Domain.Entities
{
    public class UserFollowingEntity
    {
        public long Id { get; set; } // Id (Primary key)
        public long UserId { get; set; } // UserId
        public long TargetUserId { get; set; } // TargetUserId
        public DateTime CreatedAt { get; set; } // CreatedAt
        public bool Active { get; set; } // Active
        public DateTime? RemovedAt { get; set; } // RemovedAt
    }
}