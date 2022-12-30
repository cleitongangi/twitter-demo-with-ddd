namespace Posterr.Domain.Interfaces.GlobalSettings
{
    /// <summary>
    /// Class to contains properties/parameters used by domain. Usualy, this parameters can come from appSettings/Web.aconfig or Database
    /// </summary>
    public interface IConfigSettings
    {
        int DailyLimitPosts { get; }
        int PaginationHomeFeedPageSize { get; }
        int PaginationUserPostsPageSize { get; }
    }
}
