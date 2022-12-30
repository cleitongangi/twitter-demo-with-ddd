using Posterr.Domain.Interfaces.GlobalSettings;

namespace Posterr.RestAPI.GlobalSettings
{
    public class ConfigSettings : IConfigSettings
    {
        public int DailyLimitPosts { get; private set; }
        public int PaginationHomeFeedPageSize { get; private set; }
        public int PaginationUserPostsPageSize { get; private set; }

        public ConfigSettings(IConfiguration configuration)
        {
            DailyLimitPosts = configuration.GetSection("AppSettings").GetValue<int>("dailyLimitPosts");
            PaginationHomeFeedPageSize = configuration.GetSection("AppSettings").GetSection("Pagination").GetValue<int>("HomeFeedPageSize");
            PaginationUserPostsPageSize = configuration.GetSection("AppSettings").GetSection("Pagination").GetValue<int>("UserPostsPageSize");
        }
    }
}
