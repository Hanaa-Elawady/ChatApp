namespace Chat.Services.Mapping.DTO_s
{
    public class MessageModel
    {
        public ConnectionDto Connection { get; set; }
        public Guid SenderId { get; set; }
        public bool Seen { get; set; } = false;
        public DateTime DateSent { get; set; }
        public string Text { get; set; }
        public string? AttachmentBase64 { get; set; }
        public string AttachmentType { get; set; }
    }
}
