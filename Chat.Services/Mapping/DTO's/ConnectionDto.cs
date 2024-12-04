namespace Chat.Services.Mapping.DTO_s
{
    public class ConnectionDto
    {
        public Guid ConnectionId { get; set; } = Guid.NewGuid();
        public Guid PersonId { get; set; }
        public string PhoneNumber { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePicture { get; set; }
        public IReadOnlyList<MessageDto> Messages { get; set; }
    }
}
