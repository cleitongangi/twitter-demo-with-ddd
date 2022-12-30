using Posterr.Domain.Core.Pagination;

namespace Posterr.Domain.Core.Extensions
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Paginate a IEnumerable data and return a PagedResult<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">Data to be paginated</param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize">Items per page</param>
        /// <returns>Return a PagedResult<T></returns>
        public static PagedResult<T> ToPaged<T>(this IEnumerable<T> list, int currentPage, int pageSize) where T : class
        {
            var skip = (currentPage - 1) * pageSize;
            var data = list
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>(data, currentPage, pageSize);
        }

        public static PagedResult<T> ConvertToPagedResult<T>(this IEnumerable<T> list, int currentPage, int pageSize) where T : class
        {
            return new PagedResult<T>(list.ToList(), currentPage, pageSize);
        }
    }
}