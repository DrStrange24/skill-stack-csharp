using Microsoft.AspNetCore.Identity;
using SkillStackCSharp.DTOs.UserDTOs;

namespace SkillStackCSharp.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(string id);
        Task<UserDTO> CreateUserAsync(CreateUserDTO user);
        Task<UserDTO> UpdateUserAsync(string id, UpdateUserDTO user);
        Task<bool> DeleteUserAsync(string id);
        Task<IdentityResult> ChangePasswordAsync(string id, string oldPassword, string newPassword);
    }
}
