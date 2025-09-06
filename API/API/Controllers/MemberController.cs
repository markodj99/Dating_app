using API.DTO;
using API.Extension;
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

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdate)
        {
            string? id = User.GetMemberId();

            var member = await _repo.GetMemberByIdWithUserAsync(id);
            if (member is null) return BadRequest("Could not get member");

            ToDto.MemberUpdateDtoToMember(member, memberUpdate);
            _repo.Update(member); // kinda optional

            if (await _repo.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update member");
        }
    }
}
