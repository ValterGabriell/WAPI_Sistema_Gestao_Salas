namespace CS800_Model_iCorp;

public class PagedList<T> : List<T> where T : class
{
    public int CurrentPage { get; set; }
    public int TotalPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public bool HasPrevius => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPage;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        CurrentPage = pageNumber;
        PageSize = pageSize;    
        TotalPage = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public static PagedList<T> ToPagedList(List<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), "Source cannot be null");
        }

        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
