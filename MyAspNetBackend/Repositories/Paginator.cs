using Microsoft.EntityFrameworkCore;

namespace MyAspNetBackend.Repositories;

public class Paginator<T>
{
    public int Page { get; private set; }
    public int Limit { get; private set; }
    public int Offset { get; private set; }

    public Paginator(int page = 1, int limit = 5)
    {
        Limit = limit < 1 ? 5 : limit;
        Page = page < 1 ? 1 : page;
        Offset = (Page - 1) * Limit;
    }

    public PagedResult<T> GetPagedData(IQueryable<T> query)
    {
        var totalRecords = query.Count();
        var items = query.Skip(Offset).Take(Limit).ToList();
        
        return new PagedResult<T>
        {
            Items = items,
            Metadata = GetMetadata(totalRecords)
        };
    }

    public async Task<PagedResult<T>> GetPagedDataAsync(IQueryable<T> query)
    {
        var totalRecords = await query.CountAsync();
        var items = await query.Skip(Offset).Take(Limit).ToListAsync();
        
        return new PagedResult<T>
        {
            Items = items,
            Metadata = GetMetadata(totalRecords)
        };
    }

    private PageMetadata GetMetadata(int totalRecords)
    {
        if (totalRecords == 0)
        {
            return new PageMetadata();
        }

        int totalPages = (int)Math.Ceiling(totalRecords / (double)Limit);
        
        return new PageMetadata
        {
            TotalRecords = totalRecords,
            FirstPage = 1,
            LastPage = totalPages,
            Page = Page,
            Limit = Limit
        };
    }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public PageMetadata Metadata { get; set; } = new PageMetadata();
}

public class PageMetadata
{
    public int TotalRecords { get; set; }
    public int FirstPage { get; set; }
    public int LastPage { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
}