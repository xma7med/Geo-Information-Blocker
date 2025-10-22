namespace GeoBlocker._Services
{

    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }

        public PagedResult(IReadOnlyList<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }

    public static class PaginationHelper
    {
       
        public static PagedResult<T> ToPagedResult<T>( IEnumerable<T> source,int page,int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            int totalCount = source.Count();
            var items = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>(items, page, pageSize, totalCount);
        }

      
        //public static PagedResult<T> ToPagedResult<T>( IQueryable<T> query,int page,int pageSize)
        //{
        //    if (page < 1) page = 1;
        //    if (pageSize < 1) pageSize = 10;

        //    int totalCount = query.Count();
        //    var items = query
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    return new PagedResult<T>(items, page, pageSize, totalCount);
        //}
    }

}
