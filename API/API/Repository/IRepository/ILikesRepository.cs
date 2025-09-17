using API.Model;
using API.Util;

namespace API.Repository.IRepository
{
    public interface ILikesRepository
    {
        Task<MemberLike?> GetMemberLikeAsync(string sourceMemberId, string targetMemberId);
        Task<PaginatedResult<Member>> GetMemberLikesAsync(LikesParams likesParams);
        Task<IReadOnlyList<string>> GetCurrentMemberLikeIdAsync(string memberId);
        void DeleteLike(MemberLike like);
        void AddLike(MemberLike like);
    }
}
