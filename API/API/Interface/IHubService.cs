using API.Repository.IRepository;
using Microsoft.AspNetCore.SignalR;

namespace API.Interface
{
    public interface IHubService
    {
        string GetUserId(in HubCallerContext context);
        string GetGroupName(string caller, string other);
        string GetOtherUserId(in HubCallerContext context);
        Task<bool> AddToGroup(string groupName, IUnitOfWork uow, string connectionId, string userId);
    }
}
