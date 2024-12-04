using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.Web.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var userId = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}
