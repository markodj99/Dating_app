using API.Data;
using API.Model;
using API.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class UnitOfWork(AppDbContext _context, UserManager<User> _userManager) : IUnitOfWork
    {
        private IAccountRepository? _accountRepository;
        private IMemberRepository? _memberRepository;
        private IMessageRepository? _messageRepository;
        private ILikesRepository? _likesRepository;

        public IAccountRepository AccountRepository => _accountRepository ??= new AccountRepository(_userManager);
        public IMemberRepository MemberRepository => _memberRepository ??= new MemberRepository(_context);
        public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(_context);
        public ILikesRepository LikesRepository => _likesRepository ??= new LikesRepository(_context);

        public async Task<bool> Complete()
        {
            try { return await _context.SaveChangesAsync() > 0; }
            catch (DbUpdateException ex) { throw new DbUpdateException("An error has occured while saving changes:", ex); }
        }

        public bool HasChanges() => _context.ChangeTracker.HasChanges();
    }
}
