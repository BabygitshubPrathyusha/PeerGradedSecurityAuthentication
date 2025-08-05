using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SecurityPeerGraded.Controllers;
using SecurityPeerGraded.Models;
using NUnit.Framework;
using Paket;

namespace SecurityPeerGraded.Tests
{
    
     [TestFixture]
    public class TestInputValidation
    {

        private AppDbContext _context;
        private AuthService _authService;
        private AuthController _authController;


        [Test]
        public void TestForSQLInjection()
        {
            var maliciousInput = new UserInputModel
            {
                Username = "admin'; DROP TABLE Users; --",
                Email = "test@example.com"
            };

            var result = _controller.Submit(maliciousInput);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestForXSS()
        {
            var maliciousInput = new UserInputModel
            {
                Username = "<script>alert('xss')</script>",
                Email = "xss@example.com"
            };

            var result = _controller.Submit(maliciousInput);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void TestValidInput()
        {
            var validInput = new UserInputModel
            {
                Username = "normal_user123",
                Email = "valid@example.com"
            };

            var result = _controller.Submit(validInput);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("AuthTestDb")
                .Options;

            _context = new AppDbContext(options);
            _authService = new AuthService(_context);
            _authController = new AuthController(_authService);

            // Seed test users
            _authService.Register("admin", "admin@example.com", "AdminPass123!", "Admin");
            _authService.Register("user", "user@example.com", "UserPass123!", "User");
        }

        [Test]
        public void TestValidLogin()
        {
            var loginModel = new LoginModel { Username = "admin", Password = "AdminPass123!" };
            var result = _authController.Login(loginModel);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void TestInvalidLogin()
        {
            var loginModel = new LoginModel { Username = "admin", Password = "WrongPassword" };
            var result = _authController.Login(loginModel);

            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }

        [Test]
        public void TestUnauthorizedRoleAccess()
        {
            // Simulated - real role checking would be in integration or controller-level test
            var user = _authService.Authenticate("user", "UserPass123!");
            Assert.Equals("User", user.Role);
        }

        [Test]
        public void TestAdminRoleAccess()
        {
            var user = _authService.Authenticate("admin", "AdminPass123!");
            Assert.Equals("Admin", user.Role);
        }
    }

}
