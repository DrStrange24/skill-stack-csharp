using System.ComponentModel.DataAnnotations;

namespace PersonalWebApp.DTOs
{
    public class UserDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
