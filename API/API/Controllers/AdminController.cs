using API.DTO;
using API.Interface;
using API.Model;
using API.Repository.IRepository;
using API.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class AdminController(IUnitOfWork _uow, IPhotoService _photoService) : BaseApiController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _uow.AccountRepository.GetUsersAsync();
            var userList = new List<object>();
            foreach (var user in users!)
            {
                var rls = await _uow.AccountRepository.GetRolesForAUserAsync(user);
                userList.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    roles = rls!.ToList()
                });
            }
            return Ok(userList); // example for an anonymous object
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{userId}")]
        [ProducesResponseType(typeof(IList<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<string>>> EditRoles(string userId, [FromQuery] string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select atleast one role.");
            var selectedRoles = roles.Split(",").ToArray();
            var user = await _uow.AccountRepository.GetUserByIdAsync(userId);
            if (user is null) return BadRequest("Could not retrieve the user.");
            var userRoles = await _uow.AccountRepository.GetRolesForAUserAsync(user);

            var result = await _uow.AccountRepository.AddToRolesAsync(user, selectedRoles.Except(userRoles!));
            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _uow.AccountRepository.RemoveFromRolesAsync(user, userRoles!.Except(selectedRoles));
            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _uow.AccountRepository.GetRolesForAUserAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult<IReadOnlyList<PhotoForApprovalDto>>> GetPhotosToModerate()
            => Ok(ToDto.PhotosToPhotoForApprovalDtos(await _uow.PhotoRepository.GetUnapprovedPhotos()));

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{photoId}")]
        public async Task<ActionResult> ApprovePhoto(int photoId)
        {
            var photo = await _uow.PhotoRepository.GetPhotoById(photoId);
            if (photo is null) return BadRequest("Could not get photo from the database.");
            photo.IsApproved = true;
            var member = await _uow.MemberRepository.GetMemberUpdateAsync(photo.MemberId);
            if (member is not null && member.ImageUrl == null)
 {
                member.ImageUrl = photo.Url;
                member.User.ImageUrl = photo.Url;
            }

            await _uow.Complete();
            return Ok();
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{photoId}")]
        public async Task<ActionResult> RejectPhoto(int photoId)
        {
            var photo = await _uow.PhotoRepository.GetPhotoById(photoId);
            if (photo is null) return BadRequest("Could not get photo from the database.");
            if (photo.PublicId is not null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Result == "ok") _uow.PhotoRepository.RemovePhoto(photo);
            }
            else _uow.PhotoRepository.RemovePhoto(photo);

            await _uow.Complete();
            return Ok();
        }
    }
}
