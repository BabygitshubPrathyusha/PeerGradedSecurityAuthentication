using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System;
using SecurityPeerGraded.Models;

namespace SecurityPeerGraded.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("/submit")]
        public IActionResult Submit(UserInputModel input)
        {
            if (!ModelState.IsValid || !IsValidInput(input.Username) || !IsValidEmail(input.Email))
            {
                return BadRequest("Invalid input.");
            }

            var user = new User
            {
                Username = Sanitize(input.Username),
                Email = Sanitize(input.Email)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User saved successfully.");
        }

        private bool IsValidInput(string input)
        {
            return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[a-zA-Z0-9_.-]+$");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private string Sanitize(string input)
        {
            return System.Net.WebUtility.HtmlEncode(input);
        }
    }

}
