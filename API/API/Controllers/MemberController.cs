using API.Model;
using API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MemberController(IMemberRepository _repo) : BaseAPIController
    {
        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetAllMembers()
        {
            return Ok(await _repo.GetMembersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetUser(string id)
        {
            var member = await _repo.GetMemberByIdAsync(id);
            if (member is null) return NotFound();
            return Ok(member);
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(await _repo.GetPhotosForMemberAsync(id));
        }
    }
}
