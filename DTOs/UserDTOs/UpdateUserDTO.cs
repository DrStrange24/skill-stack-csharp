namespace SkillStackCSharp.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public List<string>? Roles { get; set; } = new List<string>();
    }
}
