using PersonalWebApp.Services.Interfaces;
using PersonalWebApp.Models;
using PersonalWebApp.Repositories.Interfaces;

namespace PersonalWebApp.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task CreateUserAsync(User user)
        {
            _userRepository.AddUser(user);
            await _userRepository.SaveChangesAsync(); // Save changes to the database
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepository.UpdateUser(user);
            await _userRepository.SaveChangesAsync(); // Save changes to the database
        }

        public async Task DeleteUserAsync(User user)
        {
            _userRepository.RemoveUser(user);
            await _userRepository.SaveChangesAsync(); // Save changes to the database
        }
    }
}
