using API.Data;
using API.Model;
using API.Repository.IRepository;
using API.Util;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class LikesRepository(AppDbContext _context) : ILikesRepository
    {
        public async Task<MemberLike?> GetMemberLikeAsync(string sourceMemberId, string targetMemberId)
            => await _context.Likes.FindAsync(sourceMemberId, targetMemberId);

        public async Task<PaginatedResult<Member>> GetMemberLikesAsync(LikesParams likesParams)
        {
            var query = _context.Likes.AsQueryable();
            IQueryable<Member> result;

            switch (likesParams.Predicate)
            {
                case "liked":
                    result = query
                        .Where(like => like.SourceMemberId.Equals(likesParams.MemberId)).
                        Select(like => like.TargetMember);
                    break;
                case "likedBy":
                    result = query
                        .Where(like => like.TargetMemberId.Equals(likesParams.MemberId)).
                        Select(like => like.SourceMember);
                    break;
                default: // mutual
                    var likeIds = await GetCurrentMemberLikeIdAsync(likesParams.MemberId);
                    result = query
                        .Where(x => x.TargetMemberId.Equals(likesParams.MemberId)
                        && likeIds.Contains(x.SourceMemberId))
                        .Select(x => x.SourceMember);
                    break;
            }

            return await PaginationHelper.CreateAsync(result, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIdAsync(string memberId)
            => await _context.Likes
                .Where(x => x.SourceMemberId.Equals(memberId))
                .Select(x => x.TargetMemberId)
                .ToListAsync();

        public void DeleteLike(MemberLike like) => _context.Likes.Remove(like);

        public void AddLike(MemberLike like) => _context.Likes.Add(like);

        public async Task<bool> SaveAllChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
