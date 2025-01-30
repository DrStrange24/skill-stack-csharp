using SkillStackCSharp.DTOs;
using SkillStackCSharp.Models;

namespace SkillStackCSharp.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(string id);
        Task CreateUserAsync(UserDTO user);
        Task UpdateUserAsync(UserDTO user);
        Task DeleteUserAsync(UserDTO user);
    }
}
