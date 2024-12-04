using Chat.Data.Entities;

namespace Chat.Repository.Specifications.MessageSpecifications
{
    public class MessageWithSpecifications : BaseSpecifications<Message>
    {
        public MessageWithSpecifications(MessageSpecifications specs) 
            : base(Message => (!specs.ConnectionId.HasValue || Message.ConnectionId == specs.ConnectionId)
                && (string.IsNullOrEmpty(specs.SearchName) || Message.Text.Trim().ToLower().Contains(specs.SearchName))
            )
        {
            //ApplyPagination(specs.PageSize * (specs.PageIndex - 1), specs.PageSize);
            AddOrderBy(M => M.DateSent);
        }
        public MessageWithSpecifications (Guid? MessageId ) :base(M => M.Id == MessageId)
        {

        }
    }
}
