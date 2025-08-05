using System.ComponentModel.DataAnnotations;

namespace SecurityPeerGraded.Models
{
    public class UserInputModel
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
