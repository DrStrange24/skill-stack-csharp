using PersonalWebApp.Services.Interfaces;
using PersonalWebApp.Models;
using PersonalWebApp.Repositories.Interfaces;
using PersonalWebApp.DTOs;
using System.Collections.Generic;

namespace PersonalWebApp.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            foreach (var user in users)
            {
                
            }
            //var userDTO = new UserDTO() { 

            //};
            return null;
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) return null;

            var userDTO = new UserDTO() { 
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
            };

            return userDTO;
        }

        public async Task CreateUserAsync(UserDTO user)
        {
            //_userRepository.AddUser(user);
            //await _userRepository.SaveChangesAsync(); // Save changes to the database
        }

        public async Task UpdateUserAsync(UserDTO user)
        {
            //_userRepository.UpdateUser(user);
            //await _userRepository.SaveChangesAsync(); // Save changes to the database
        }

        public async Task DeleteUserAsync(UserDTO user)
        {
            //_userRepository.RemoveUser(user);
            //await _userRepository.SaveChangesAsync(); // Save changes to the database
        }
    }
}
