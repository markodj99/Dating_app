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
    public class LikeController(IUnitOfWork _uow) : BaseApiController
    {
        [HttpPost("{targetMemberId}")]
        public async Task<ActionResult> ToggleLike(string targetMemberId)
        {
            var sourceMemberId = User.GetMemberId();
            if (sourceMemberId.Equals(targetMemberId)) return BadRequest("You can not like yourself.");

            var existingLike = await _uow.LikesRepository.GetMemberLikeAsync(sourceMemberId, targetMemberId);
            if (existingLike is null)
            {
                var like = new MemberLike
                {
                    SourceMemberId = sourceMemberId,
                    TargetMemberId = targetMemberId
                };
                _uow.LikesRepository.AddLike(like);
            }
            else _uow.LikesRepository.DeleteLike(existingLike);

            if (await _uow.Complete()) return Ok();
            return BadRequest("Failed to update a like.");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetCurrentMemberLikeIds()
            => Ok(await _uow.LikesRepository.GetCurrentMemberLikeIdAsync(User.GetMemberId()));

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<MemberDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResult<MemberDto>>> GetMemberLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.MemberId = User.GetMemberId();
            return Ok(ToDto.PRMemberToPRMemberDto(await _uow.LikesRepository.GetMemberLikesAsync(likesParams)));
        }
    }
}
