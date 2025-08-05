using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Paket;

namespace SecurityPeerGraded.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            var user = _authService.Authenticate(login.Username, login.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            // Simulate issuing token or session
            return Ok(new { Username = user.Username, Role = user.Role });
        }
    }

}
