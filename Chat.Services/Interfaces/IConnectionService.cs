using Chat.Services.Mapping.DTO_s;

namespace Chat.Services.Interfaces
{
    public interface IConnectionService
    {
        Task<IReadOnlyList<ConnectionDto>> getAllContacts(string UserId);
        Task<ConnectionDto> GetConnection(string UserId);
        Task<bool> AddNewContact(string PhoneNumber, string UserId);
    }
}
