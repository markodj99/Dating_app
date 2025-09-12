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

        public Task<PaginatedResult<Message>> GetMessagesForMemberAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Message>> GetMessageThreadAsync(string currentMemberId, string recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
