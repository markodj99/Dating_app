using API.DTO;
using API.Extension;
using API.Model;
using API.Repository.IRepository;
using API.Util;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController(IMessageRepository _messageRepo, IMemberRepository _memberRepo) : BaseAPIController
    {
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var sender = await _memberRepo.GetMemberByIdAsync(User.GetMemberId());
            var recipient = await _memberRepo.GetMemberByIdAsync(createMessageDto.RecipientId);


            if (recipient == null || sender == null 
                || sender.Id.Equals(createMessageDto.RecipientId)) return BadRequest("Invalid sender or recipient.");

            var message = new Message
            {
                Content = createMessageDto.Content,
                MessageSent = DateTime.UtcNow,
                SenderDeleted = false,
                RecipientDeleted = false,
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = createMessageDto.RecipientId,
                Recipient = recipient,
            };

            _messageRepo.AddMessage(message);
            if (await _messageRepo.SaveAllAsync()) return Ok(ToDto.MessageToMessageDto(message));
            return BadRequest("Could not send message.");
        }
    }
}
