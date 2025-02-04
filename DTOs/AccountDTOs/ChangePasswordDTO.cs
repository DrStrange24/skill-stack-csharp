using System.ComponentModel.DataAnnotations;

namespace SkillStackCSharp.DTOs.AccountDTOs
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Current password is required.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}
