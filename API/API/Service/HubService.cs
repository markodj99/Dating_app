using API.Extension;
using API.Interface;
using API.Model;
using API.Repository.IRepository;
using Microsoft.AspNetCore.SignalR;

namespace API.Service
{
    public class HubService : IHubService
    {
        public string GetUserId(in HubCallerContext context) => context.User?.GetMemberId() ?? throw new HubException("Cannot get member id.");

        public string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        public string GetOtherUserId(in HubCallerContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext?.Request?.Query["userId"].ToString() ?? throw new HubException("Other user not found.");
        }

        public async Task<bool> AddToGroup(string groupName, IUnitOfWork uow, string connectionId, string userId)
        {
            var group = await uow.MessageRepository.GetMessageGroupAsync(groupName);
            var connection = new Connection(connectionId, userId);

            if (group is null)
            {
                group = new Group(groupName);
                uow.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);
            return await uow.Complete();
        }
    }
}
