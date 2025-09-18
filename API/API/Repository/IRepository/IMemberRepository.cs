using API.Model;
using API.Util;

namespace API.Repository.IRepository
{
    public interface IMemberRepository
    {
        void Update(Member member);
        Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams);
        Task<Member?> GetMemberByIdAsync(string id);
        Task<Member?> GetMemberUpdateAsync(string id);
        Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId, bool isCurrentUser);
    }
}
