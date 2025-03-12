namespace ShiftSwift.Shared.paging
{
    public record PaginatedRequest
    {
        public int PageNumber { get; set; } = 0;
        private int pageSize = 5;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > 10 ? 10 : value;
        }
        public string? SortBy { get; set; }
        public string SortOrder { get; set; }
    }
}
