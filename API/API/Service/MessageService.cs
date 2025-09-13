using API.DTO;
using API.Interface;
using API.Model;

namespace API.Service
{
    public class MessageService : IMessageService
    {
        public Message CreateNewMessage(CreateMessageDto createMessageDto, string senderId)
            => new()
            {
                Content = createMessageDto.Content,
                MessageSent = DateTime.UtcNow,
                SenderDeleted = false,
                RecipientDeleted = false,
                SenderId = senderId,
                RecipientId = createMessageDto.RecipientId,
            };
    }
}
