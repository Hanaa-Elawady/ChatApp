namespace Chat.Services.Mapping.DTO_s
{
    public class MessageDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ConnectionId { get; set; }
        public Guid SenderId { get; set; }
        public bool Seen { get; set; } = false;
        public DateTime DateSent { get; set; }
        public DateTime? DateSeen { get; set; }
        public string Text { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? AttachmentType { get; set; }

    }
}
