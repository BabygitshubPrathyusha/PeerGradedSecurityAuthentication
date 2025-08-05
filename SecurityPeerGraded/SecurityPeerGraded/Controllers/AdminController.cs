using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecurityPeerGraded.Controllers
{

    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            return Ok("Welcome to the Admin Dashboard");
        }
    }
}
