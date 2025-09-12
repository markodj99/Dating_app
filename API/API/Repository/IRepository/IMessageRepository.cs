using API.Model;
using API.Util;

namespace API.Repository.IRepository
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message?> GetMessageAsync(string messageId);
        Task<PaginatedResult<Message>> GetMessagesForMemberAsync();
        Task<IReadOnlyList<Message>> GetMessageThreadAsync(string currentMemberId, string recipientId);
        Task<bool> SaveAllChangesAsync();
    }
}
