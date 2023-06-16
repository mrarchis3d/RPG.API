using Microsoft.EntityFrameworkCore;

namespace RPG.BuildingBlocks.Common.Utils
{
    public class PagedHelper<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public List<T> Items { get; set; }

        public PagedHelper(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public static PagedHelper<T> ToPageList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedHelper<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PagedHelper<T>> ToPageListAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedHelper<T>(items, count, pageNumber, pageSize);
        }

        //public static async Task<PagedHelper<T>> ToPageListAsync<TK>(IQueryable<TK> source, int pageNumber, int pageSize, IMapper mapper) where TK : class
        //{
        //    var count = await source.CountAsync();
        //    var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        //    var mapped = mapper.Map<List<T>>(items);
        //    return new PagedHelper<T>(mapped, count, pageNumber, pageSize);
        //}

        public static PagedHelper<T> AsPagedList(IQueryable<T> items, int pageNumber, int pageSize, int count) 
            => new(items.ToList(), items.Count() > 0 ? items.Count() : 1, pageNumber, pageSize);
    }
}
