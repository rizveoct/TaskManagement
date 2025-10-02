namespace TaskManagement.Application.Common.Models;

public class PagedList<T>
{
    private PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items.ToList();
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public IReadOnlyCollection<T> Items { get; }
    public int TotalCount { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public static PagedList<T> Create(IEnumerable<T> source, int totalCount, int pageNumber, int pageSize) =>
        new(source, totalCount, pageNumber, pageSize);
}
