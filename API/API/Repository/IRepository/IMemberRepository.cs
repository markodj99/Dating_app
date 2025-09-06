using API.Model;

namespace API.Repository.IRepository
{
    public interface IMemberRepository
    {
        void Update(Member member);
        Task<bool> SaveAllAsync();
        Task<IReadOnlyList<Member>> GetMembersAsync();
        Task<Member?> GetMemberByIdAsync(string id);
        Task<Member?> GetMemberByIdWithUserAsync(string id);
        Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId);
    }
}
