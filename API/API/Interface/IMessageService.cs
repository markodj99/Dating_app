using API.DTO;
using API.Model;

namespace API.Interface
{
    public interface IMessageService
    {
        Message CreateNewMessage(CreateMessageDto createMessageDto, string senderId);
    }
}
