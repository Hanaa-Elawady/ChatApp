using Chat.Data.Entities;
using Chat.Services.Interfaces;
using Chat.Services.Mapping.DTO_s;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IMessagesService _messagesService;
        private readonly IConnectionService _connectionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHub(ILogger<ChatHub> logger, IMessagesService messagesService, IConnectionService connectionService , UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _messagesService = messagesService;
            _connectionService = connectionService;
            _userManager = userManager;
        }

        private async Task<string> SaveFileAsync(string base64Data, string folder, string fileExtension)
        {
            try
            {
                var fileData = Convert.FromBase64String(base64Data.Split(',')[1]);
                var fileName = $"{Guid.NewGuid()}{fileExtension}"; // Unique file name
                var filePath = Path.Combine("wwwroot/Assests", folder, fileName);

                // Ensure the directory exists
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                await File.WriteAllBytesAsync(filePath, fileData);

                return $"/Assests/{folder}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving file");
                throw new InvalidOperationException("Error saving file", ex);
            }
        }

        public async Task Send(MessageModel messageObject)
        {
            string attachmentUrl = null;

            if (messageObject.AttachmentType != "text")
            {
                var Extenssion = messageObject.AttachmentType == "Audios" ? ".mp3"
                                : messageObject.AttachmentType == "Videos" ?".mp4" 
                                : messageObject.AttachmentType == "PDFs" ?".pdf" 
                                : messageObject.AttachmentType == "Photos" ? ".png" : "";
                attachmentUrl = await SaveFileAsync(messageObject.AttachmentBase64, messageObject.AttachmentType,Extenssion);
            }

            var message = new MessageDto
            {
                Id = Guid.NewGuid(),
                ConnectionId = messageObject.Connection.ConnectionId,
                SenderId = messageObject.SenderId,
                Seen = false,
                DateSent = messageObject.DateSent,
                Text = messageObject.Text,
                AttachmentUrl = attachmentUrl,
                AttachmentType = messageObject.AttachmentType,
            };

            try
            {
                await _messagesService.AddAsync(message);
                await Clients.User(messageObject.SenderId.ToString()).SendAsync("ReceiveMessage", message);
                await Clients.User(messageObject.Connection.PersonId.ToString()).SendAsync("ReceiveMessage", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
            }
        }



        #region voice call
        public async Task SendOffer(Guid connection, Guid userId,string offer)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            
            await Clients.User(connection.ToString()).SendAsync("ReceiveOffer", offer , user);
        }

        public async Task SendAnswer(Guid connection, string answer)
        {
            await Clients.User(connection.ToString()).SendAsync("ReceiveAnswer", answer);
        }

        public async Task SendIceCandidate( Guid connection, string candidate)
        {
            await Clients.User(connection.ToString()).SendAsync("ReceiveIceCandidate", candidate);
        }
        public async Task SendDecline(Guid connection)
        {
            await Clients.User(connection.ToString()).SendAsync("CallDeclinedNotification");
        }
        public async Task SendNoRespond(Guid connection)
        {
            await Clients.User(connection.ToString()).SendAsync("NoResponse");
        }
        #endregion
    }
}

