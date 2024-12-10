using Chat.Data.Entities;

namespace Chat.Repository.Specifications.ConnectionSepecifications
{
    public class UserConnectionsWithSpecifications : BaseSpecifications<UserConnection>
    {
        public UserConnectionsWithSpecifications(UserConnectionsSpecifications specs)
            : base(connection => (!specs.UserId.HasValue || connection.User1Id == specs.UserId.Value || connection.User2Id == specs.UserId.Value)
            )
        {
           
            AddInclude(x => x.User1);
            AddInclude(x => x.User2);
            AddInclude(x =>x.Messages);

        }
        public UserConnectionsWithSpecifications(Guid? id) : base(connection => connection.User1Id == id || connection.User2Id == id)

        {
            AddInclude(x => x.User1);
            AddInclude(x => x.User2);

        }
        public UserConnectionsWithSpecifications(string? name) : base(connection => string.IsNullOrEmpty(name) || connection.User1.ToString().Trim().ToLower().Contains(name))
        {
            AddInclude(x => x.User1);
            AddInclude(x => x.User2);
        }
    }
}
