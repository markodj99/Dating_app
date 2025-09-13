using API.Data;
using API.Model;
using API.Repository.IRepository;
using API.Util;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MessageRepository(AppDbContext _context) : IMessageRepository
    {
        public void AddMessage(Message message) => _context.Messages.Add(message);

        public void DeleteMessage(Message message) => _context.Messages.Remove(message);

        public async Task<Message?> GetMessageAsync(string messageId) => await _context.Messages.FindAsync(messageId);

        public async Task<PaginatedResult<Message>> GetMessagesForMemberAsync(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(x => x.MessageSent).Include(x => x.Sender).Include(x => x.Recipient).AsQueryable();
            query = messageParams.Container switch
            {
                "Outbox" => query.Where(x => x.SenderId.Equals(messageParams.MemberId) && !x.SenderDeleted),
                _ => query.Where(x => x.RecipientId.Equals(messageParams.MemberId) && !x.RecipientDeleted) // Inbox
            };
            return await PaginationHelper.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IReadOnlyList<Message>> GetMessageThreadAsync(string currentMemberId, string recipientId)
        {
            await _context.Messages
                .Where(x => x.RecipientId.Equals(currentMemberId)
                && x.SenderId.Equals(recipientId) && x.DateRead == null)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.DateRead, DateTime.UtcNow));

            return await _context.Messages
                .Where(x => (x.RecipientId.Equals(currentMemberId) && !x.RecipientDeleted && x.SenderId.Equals(recipientId))
                || (x.SenderId.Equals(currentMemberId) && !x.SenderDeleted && x.RecipientId.Equals(recipientId)))
                .OrderBy(x => x.MessageSent).Include(x => x.Sender).Include(x => x.Recipient)
                .ToListAsync();
        }

        public async Task<bool> SaveAllChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
