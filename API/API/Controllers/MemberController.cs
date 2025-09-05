using API.DTO;
using API.Model;
using API.Repository.IRepository;
using API.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MemberController(IMemberRepository _repo) : BaseAPIController
    {
        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyList<MemberDto>>> GetAllMembers()
        {
            return Ok(ToDto.MembersToMemberDtos(await _repo.GetMembersAsync()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(string id)
        {
            var member = await _repo.GetMemberByIdAsync(id);
            if (member is null) return NotFound();
            return Ok(ToDto.MemberToMemberDto(member));
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(ToDto.PhotosToPhotoDtos(await _repo.GetPhotosForMemberAsync(id)));
        }
    }
}
