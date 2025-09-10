using API.DTO;
using API.Extension;
using API.Interface;
using API.Model;
using API.Repository.IRepository;
using API.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MemberController(IMemberRepository _repo, IPhotoService _photoService) : BaseAPIController
    {
        [HttpGet("all")]
        [ProducesResponseType(typeof(PaginatedResult<MemberDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResult<MemberDto>>> GetAllMembers([FromQuery] MemberParams memberParams)
        {
            memberParams.CurrentMemberId = User.GetMemberId();

            return Ok(ToDto.PRMemberToPRMemberDto(await _repo.GetMembersAsync(memberParams)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(string id)
        {
            var member = await _repo.GetMemberByIdAsync(id);
            if (member is null) return NotFound();
            return Ok(ToDto.MemberToMemberDto(member));
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<PhotoDto>>> GetMemberPhotos(string id)
        {
            return Ok(ToDto.PhotosToPhotoDtos(await _repo.GetPhotosForMemberAsync(id)));
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdate)
        {
            var member = await GetMember();
            if (member is null) return BadRequest("Could not get member");

            ToDto.MemberUpdateDtoToMember(member, memberUpdate);
            _repo.Update(member); // kinda optional

            if (await _repo.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update member");
        }

        [HttpPost("add-photo")]
        [ProducesResponseType(typeof(PhotoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PhotoDto>> AddPhoto([FromForm] AddPhotoDto dto)
        {
            var file = dto.File;
            var member = await GetMember();
            if (member is null) return BadRequest("Could not get member");

            var result = await _photoService.UploadPhotoAsync(file);
            if (result.Error is not null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                MemberId = member.Id
            };

            if (member.ImageUrl is null)
            {
                member.ImageUrl = photo.Url;
                member.User.ImageUrl = photo.Url;
            }

            member.Photos.Add(photo);

            if (await _repo.SaveAllAsync()) return Ok(ToDto.PhotoToPhotoDto(photo));
            return BadRequest("Problem adding ptoto");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var member = await GetMember();
            if (member is null) return BadRequest("Could not get member");

            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
            if (member.ImageUrl == photo?.Url || photo is null) return BadRequest("Can not set this as main image");

            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;

            if (await _repo.SaveAllAsync()) return NoContent();
            return BadRequest("Something went wrong");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var member = await GetMember();
            if (member is null) return BadRequest("Could not get member");

            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
            if (photo is null || photo.Url == member.ImageUrl) return BadRequest("Could not find photo or it is main photo");

            if (photo.PublicId is not null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error is not null) return BadRequest(result.Error.Message);
            }

            member.Photos.Remove(photo);
            if (await _repo.SaveAllAsync()) return Ok();
            return BadRequest("Problem deleting a photo");
        }

        private async Task<Member?> GetMember()
        {
            string id = User.GetMemberId();
            return await _repo.GetMemberUpdateAsync(id);
        }
    }
}
