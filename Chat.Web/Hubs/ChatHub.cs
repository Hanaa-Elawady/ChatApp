using Chat.Services.Interfaces;
using Chat.Services.Mapping.DTO_s;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IMessagesService _messagesService;
        private readonly IConnectionService _connectionService;

        public ChatHub(ILogger<ChatHub> logger, IMessagesService messagesService, IConnectionService connectionService)
        {
            _logger = logger;
            _messagesService = messagesService;
            _connectionService = connectionService;
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
        public async Task SendOffer(ConnectionDto connection, string offer)
        {
            await Clients.User(connection.PersonId.ToString()).SendAsync("ReceiveOffer", offer);
        }

        public async Task SendAnswer(ConnectionDto connection , string answer)
        {
            await Clients.User(connection.PersonId.ToString()).SendAsync("ReceiveAnswer", answer);
        }

        public async Task SendIceCandidate( ConnectionDto connection, string candidate)
        {
            await Clients.User(connection.PersonId.ToString()).SendAsync("ReceiveIceCandidate", candidate);
        }
        public async Task SendDecline(ConnectionDto connection)
        {
            await Clients.User(connection.PersonId.ToString()).SendAsync("CallDeclinedNotification");
        }
        #endregion
    }
}

