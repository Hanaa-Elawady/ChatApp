namespace Chat.Repository.Specifications.MessageSpecifications
{
    public class MessageSpecifications
    {
        public Guid? ConnectionId { get; set; }

        private string? _Search;
        public string? SearchName
        {
            get => _Search;
            set => _Search = value?.Trim().ToLower();
        }

        private const int MAXPAGESIZE = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 6;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAXPAGESIZE) ? MAXPAGESIZE : value;
        }
    }
}
