using API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class AdminController(IUnitOfWork _uow) : BaseApiController
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
        public ActionResult GetPhotosToModerate()
        {
            return Ok("Only admins or moderators can see this.");
        }
    }
}
