using API.Data;
using API.Model;
using API.Repository.IRepository;
using API.Util;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MemberRepository(AppDbContext _context) : IMemberRepository
    {
        public void Update(Member member) => _context.Entry(member).State = EntityState.Modified;

        public async Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams)
        {
            var query = _context.Members.AsQueryable();
            query = query.Where(x => !x.Id.Equals(memberParams.CurrentMemberId));
            if (memberParams.Gender is not null) query = query.Where(x => x.Gender.Equals(memberParams.Gender));

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParams.MinAge));
            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            query = memberParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive)
            };

            return await PaginationHelper.CreateAsync(query, memberParams.PageNumber, memberParams.PageSize);
        }

        public async Task<Member?> GetMemberByIdAsync(string id) => await _context.Members.FindAsync(id);

        public async Task<Member?> GetMemberUpdateAsync(string id)
            => await _context.Members
                .Include(x => x.User)
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

        public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId)
            => await _context.Members.Where(x => x.Id.Equals(memberId))
                .SelectMany(x => x.Photos).ToListAsync();
    }
}
