namespace MediatrExample
    public class PagedList<T> : List<T>
    {
        public PagedList()
        {

        }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (items != null)
            {
                AddRange(items);
            }
        }

        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        

        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = count > 0 ? source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList() : null;
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
