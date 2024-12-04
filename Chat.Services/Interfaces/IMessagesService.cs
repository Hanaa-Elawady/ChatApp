using Chat.Services.Mapping.DTO_s;

namespace Chat.Services.Interfaces
{
    public interface IMessagesService
    {
        Task<IReadOnlyList<MessageDto>> GetChatMessgages(Guid ChatId);
        Task AddAsync(MessageDto messageDto);
    }
}
