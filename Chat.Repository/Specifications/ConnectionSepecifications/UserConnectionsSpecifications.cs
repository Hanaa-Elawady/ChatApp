namespace Chat.Repository.Specifications.ConnectionSepecifications
{
    public class UserConnectionsSpecifications
    {
        public Guid? UserId { get; set; }

        private string? _Search;
        public string? SearchName
        {
            get => _Search;
            set => _Search = value?.Trim().ToLower();
        }
    }
}
