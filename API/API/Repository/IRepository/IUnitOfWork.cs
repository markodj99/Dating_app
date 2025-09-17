namespace API.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IMemberRepository MemberRepository { get; }
        IMessageRepository MessageRepository { get; }
        ILikesRepository LikesRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
