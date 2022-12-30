namespace Posterr.Domain.Core.Pagination
{
    public class PagedResult<T> where T : class
    {
        public IList<T> Results { get; private set; }
        public int CurrentPage { get; private set; }        
        public int PageSize { get; private set; }        
        
        public PagedResult(IList<T> results, int currentPage, int pageSize)
        {
            Results = results;
            CurrentPage = currentPage;
            PageSize = pageSize;            
        }
    }
}
