using System.ComponentModel.DataAnnotations;

namespace SkillStackCSharp.DTOs.UserDTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
