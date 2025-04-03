namespace WAPI_GS.Utilidades
{
    public class PaginationModel<T>
    {
        public required List<T> List { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevius { get; set; }
        public bool HasNext { get; set; }
    }
}
