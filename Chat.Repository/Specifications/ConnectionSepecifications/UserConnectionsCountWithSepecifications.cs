using Chat.Data.Entities;

namespace Chat.Repository.Specifications.ConnectionSepecifications
{
    public class UserConnectionsCountWithSepecifications :BaseSpecifications<UserConnection>
    {
        public UserConnectionsCountWithSepecifications(UserConnectionsSpecifications specs)
                : base(connection => (!specs.UserId.HasValue || connection.User1Id == specs.UserId.Value)
                )
        {

        }
    }
}
