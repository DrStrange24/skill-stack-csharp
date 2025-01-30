using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SkillStackCSharp.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public bool EmailConfirmed { get; set; }

    }
}
