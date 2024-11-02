namespace ElectronicShop.Model
{
    public class PagingRequest
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? Keyword { get; set; }
    }

    public class PagingResponse<T>
    {
        public int TotalRecords { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize == 0 ? 0 : (TotalRecords + PageSize - 1) / PageSize;
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    }

    public class PagingResponseWeb<T>
    {
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public Paginate Paginate { get; set; }
    }

    public class Paginate
    {
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public int TotalPages => PerPage == 0 ? 0 : (Count + PerPage - 1) / PerPage;
        public int? NextPage => (CurrentPage + 1) >= TotalPages ? null : (CurrentPage + 1);
        public int? PreviousPage => (CurrentPage - 1) < 1 ? null : (CurrentPage - 1);
    }
}