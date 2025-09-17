using API.DTO;
using API.Interface;
using API.Repository.IRepository;
using API.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class MessageHub(IUnitOfWork _uow, IMessageService _msgService, IHubService _hubService, IHubContext<PresenceHub> _presenceHub) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = _hubService.GetUserId(Context);
            var otherUserId = _hubService.GetOtherUserId(Context);

            var groupName = _hubService.GetGroupName(userId, otherUserId);
            await _hubService.AddToGroup(groupName, _uow, Context.ConnectionId, userId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var messages = await _uow.MessageRepository.GetMessageThreadAsync(userId, otherUserId);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", ToDto.MessagesToMessageDtos(messages)); 
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var sender = await _uow.MemberRepository.GetMemberByIdAsync(_hubService.GetUserId(Context));
            var recipient = await _uow.MemberRepository.GetMemberByIdAsync(createMessageDto.RecipientId);

            if (recipient == null || sender == null
                || sender.Id.Equals(createMessageDto.RecipientId)) throw new HubException("Cannot send the message.");

            var message = _msgService.CreateNewMessage(createMessageDto, sender.Id);
            var groupName = _hubService.GetGroupName(sender.Id, recipient.Id);
            var group = await _uow.MessageRepository.GetMessageGroupAsync(groupName);
            bool userInGroop = group is not null && group.Connections.Any(x => x.UserId == recipient.Id);
            if (userInGroop) message.DateRead = DateTime.UtcNow;

            _uow.MessageRepository.AddMessage(message);
            if (await _uow.Complete())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", ToDto.MessageToMessageDto(message));
                var connections = await PresenceTracker.GetConnectionsForUser(recipient.Id);
                if (connections is not null && connections.Count > 0 && !userInGroop)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", ToDto.MessageToMessageDto(message));
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _uow.MessageRepository.RemoveConnectionAsync(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
