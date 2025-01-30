using System.ComponentModel.DataAnnotations;

namespace SkillStackCSharp.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

}
