using AutoMapper;
using Chat.Data.Contexts;
using Chat.Data.Entities;
using Chat.Repository.Interfaces;
using Chat.Repository.Specifications.ConnectionSepecifications;
using Chat.Services.Interfaces;
using Chat.Services.Mapping.DTO_s;
using Microsoft.AspNetCore.Identity;

namespace Chat.Services.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ChatDbContext _context;

        public ConnectionService(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManager ,IMapper mapper,ChatDbContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> AddNewContact(string PhoneNumber,string UserId)
        {
            var AllUserConnections = await getAllContacts(UserId);
            var user = await _userManager.FindByNameAsync(PhoneNumber);
            if(AllUserConnections.Any(c => c.PhoneNumber == PhoneNumber)){
                return false;
            }
            else
            {

                var connection = new UserConnection()
                {
                    Id = Guid.NewGuid(),
                    User1Id = Guid.Parse(UserId),
                    User2Id = user.Id,
                    CreatedAt = DateTime.Now,
                };
                await  _unitOfWork.Repository<UserConnection>().AddAsync(connection);
                await _unitOfWork.CompleteAsync();
                return true;

            }

        }

        public async Task<IReadOnlyList<ConnectionDto>> getAllContacts(string UserId)
        {
            var Info = new UserConnectionsSpecifications() { UserId = Guid.Parse(UserId)};
            var specs = new UserConnectionsWithSpecifications(Info);

            IReadOnlyList<UserConnection> connections = await _unitOfWork.Repository<UserConnection>().GetAllAsNoTrackingAsync(specs);

            var RefactoredConnections = connections.Select(c => new UserConnection
            {
                Id = c.Id,
                User1 = (c.User1Id == Guid.Parse(UserId)) ? c.User1 : c.User2,
                User2 = (c.User1Id == Guid.Parse(UserId)) ? c.User2 : c.User1,
                Messages = c.Messages
            });

            var maappedConnections = _mapper.Map<IReadOnlyList<ConnectionDto>>(RefactoredConnections);
            return maappedConnections;
        }

        public async Task<ConnectionDto> GetConnection(string UserId)
        {
            var connection = await _unitOfWork.Repository<UserConnection>().GetAllAsNoTrackingAsync(new UserConnectionsWithSpecifications(Guid.Parse(UserId)));
            var RefactorConnection = connection.Select(c => new UserConnection
            {
                Id = c.Id,
                User1 = (c.User1Id == Guid.Parse(UserId)) ? c.User1 : c.User2,
                User2 = (c.User1Id == Guid.Parse(UserId)) ? c.User2 : c.User1
            }).FirstOrDefault();
            var mappedConnection = _mapper.Map<ConnectionDto>(RefactorConnection);
            return mappedConnection;
        }
    }
}
