namespace api.Helpers
{
    public class QueryObject
    {
        public string? search { get; set; } = null;
        public int? department { get; set; } = null;
        public int? sector { get; set; } = null;
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 25;
    }
}
