using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FaultyController : BaseApiController
    {
        [HttpGet("auth")]
        public IActionResult GetAuth() => Unauthorized();

        [HttpGet("not-found")]
        public IActionResult GetNotFound() => NotFound();

        [HttpGet("server-error")]
        public IActionResult GetServerError() => throw new Exception("Internal server error.");

        [HttpGet("bad-request")]
        public IActionResult GetBadRequest() => BadRequest();

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-secret")]
        public IActionResult GetSecretAdmin() => Ok("Only admins should see this.");
    }
}
