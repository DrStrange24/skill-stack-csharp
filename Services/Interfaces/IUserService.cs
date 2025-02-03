using DTOs.UserDTOs.CreateUserDTO;
using DTOs.UserDTOs.UserDTO;

namespace SkillStackCSharp.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(string id);
        Task<UserDTO> CreateUserAsync(CreateUserDTO user);
        Task UpdateUserAsync(UserDTO user);
        Task DeleteUserAsync(UserDTO user);
    }
}
