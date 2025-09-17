using API.DTO;
using API.Extension;
using API.Interface;
using API.Repository.IRepository;
using API.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessageController(IUnitOfWork _uow, IMessageService _msgService) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var sender = await _uow.MemberRepository.GetMemberByIdAsync(User.GetMemberId());
            var recipient = await _uow.MemberRepository.GetMemberByIdAsync(createMessageDto.RecipientId);

            if (recipient == null || sender == null 
                || sender.Id.Equals(createMessageDto.RecipientId)) return BadRequest("Invalid sender or recipient.");

            var message = _msgService.CreateNewMessage(createMessageDto, sender.Id);
            _uow.MessageRepository.AddMessage(message);
            if (await _uow.Complete()) return Ok(ToDto.MessageToMessageDto(message));
            return BadRequest("Could not send a message.");
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<MessageDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResult<MessageDto>>> GetMessagesByContainer([FromQuery] MessageParams messageParams)
        {
            messageParams.MemberId = User.GetMemberId();
            return Ok(ToDto.PRMessageToPRMessageDto(await _uow.MessageRepository.GetMessagesForMemberAsync(messageParams)));
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessageThread(string recipientId)
            => Ok(ToDto.MessagesToMessageDtos(await _uow.MessageRepository.GetMessageThreadAsync(User.GetMemberId(), recipientId)));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(string id)
        {
            var memberId = User.GetMemberId();
            var message = await _uow.MessageRepository.GetMessageAsync(id);
            if (message is null) return BadRequest("Cannot delete this message.");
            if (message.SenderId != memberId && message.RecipientId != memberId) return BadRequest("You cannot delete this message.");
            if (message.SenderId == memberId) message.SenderDeleted = true;
            if (message.RecipientId == memberId) message.RecipientDeleted = true;
            if (message is { SenderDeleted: true, RecipientDeleted: true }) _uow.MessageRepository.DeleteMessage(message);
            if (await _uow.Complete()) return Ok();
            return BadRequest("Problem deleting the message.");
        }
    }
}
