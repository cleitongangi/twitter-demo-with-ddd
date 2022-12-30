using Posterr.Domain.Entities;

namespace Posterr.RestAPI.ApiResponses
{
    public class GetFeedPostsResponse
    {
        public long Id { get; set; }
        public string? PostType { get; set; }
        public string? Text { get; set; }        
        public DateTime CreatedAt { get; set; }
        public long UserId { get; set; }
        public int TypeId { get; set; }
        public ParentPost? Parent { get; set; }

        public class ParentPost
        {
            public long Id { get; set; }
            public string? Text { get; set; }
            public DateTime CreatedAt { get; set; }
            public long UserId { get; set; }
        }
    }
}
