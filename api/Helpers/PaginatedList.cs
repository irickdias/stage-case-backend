namespace api.Helpers
{
    public class PaginatedList<T>
    {
        public List<T> data { get; set; } = new();
        public int totalPages { get; set; }
        public int totalRecords { get; set; }
    }
}
