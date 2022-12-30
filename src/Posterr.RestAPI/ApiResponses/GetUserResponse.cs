namespace Posterr.RestAPI.ApiResponses
{
    public class GetUserResponse
    {
        public long UserId { get; }
        public string Username { get; }
        public string JoinedAt { get; }
        public int FollowersCount { get; }
        public int FollowingCount { get; }
        public int PostsCount { get; }
        public bool Following { get; }

        public GetUserResponse(long userId, string username, string joinedAt, int followersCount, int followingCount, int postsCount, bool following)
        {
            UserId = userId;
            Username = username;
            JoinedAt = joinedAt;            
            FollowersCount = followersCount;
            FollowingCount = followingCount;
            PostsCount = postsCount;
            Following = following;
        }
    }
}
