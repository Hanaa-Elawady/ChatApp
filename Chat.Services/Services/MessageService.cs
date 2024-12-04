using AutoMapper;
using Chat.Data.Entities;
using Chat.Repository.Interfaces;
using Chat.Repository.Specifications.MessageSpecifications;
using Chat.Services.Interfaces;
using Chat.Services.Mapping.DTO_s;

namespace Chat.Services.Services
{
    public class MessageService : IMessagesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(MessageDto messageDto)
        {
            var message = _mapper.Map<Message>(messageDto);
            await _unitOfWork.Repository<Message>().AddAsync(message);
            var result = await _unitOfWork.CompleteAsync();
        }

        public async Task<IReadOnlyList<MessageDto>> GetChatMessgages(Guid ChatId)
        {
            var info = new MessageSpecifications() { ConnectionId = ChatId };
            var specs = new MessageWithSpecifications(info);
            IReadOnlyList<Message> messages = await _unitOfWork.Repository<Message>().GetAllAsNoTrackingAsync(specs);

            var mappedMessages = _mapper.Map<IReadOnlyList<MessageDto>>(messages);
            return mappedMessages;
        }
    }
}
