using API.Model;
using API.Util;

namespace API.Repository.IRepository
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message?> GetMessageAsync(string messageId);
        Task<PaginatedResult<Message>> GetMessagesForMemberAsync(MessageParams messageParams);
        Task<IReadOnlyList<Message>> GetMessageThreadAsync(string currentMemberId, string recipientId);
        Task<bool> SaveAllChangesAsync();
        void AddGroup(Group group);
        Task RemoveConnectionAsync(string connectionId);
        Task<Connection?> GetConnectionAsync(string connectionId);
        Task<Group?> GetMessageGroupAsync(string groupName);
        Task<Group?> GetGroupForConnectionAsync(string connectionId);
    }
}
