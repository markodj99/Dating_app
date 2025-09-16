using API.Interface;
using API.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub(PresenceTracker _presenceTracker, IHubService _hubService) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await _presenceTracker.UserConnected(_hubService.GetUserId(Context), Context.ConnectionId);
            await Clients.Others.SendAsync("UserOnline", _hubService.GetUserId(Context));

            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _presenceTracker.UserDisconnected(_hubService.GetUserId(Context), Context.ConnectionId);
            await Clients.Others.SendAsync("UserOffline", _hubService.GetUserId(Context));

            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }
    }
}
