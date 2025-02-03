using SkillStackCSharp.Services.Interfaces;
using SkillStackCSharp.Repositories.Interfaces;
using SkillStackCSharp.Models;
using SkillStackCSharp.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using SkillStackCSharp.Constants;

namespace SkillStackCSharp.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository userRepository, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            // Map User entities to UserDTO
            var userDto = users.Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName =  user.LastName,
                EmailConfirmed = user.EmailConfirmed
            });

            return userDto;
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
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
            };

            return userDTO;
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            var user = new User()
            {
                UserName = createUserDTO.UserName,
                Email = createUserDTO.Email,
                FirstName = createUserDTO.FirstName,
                LastName = createUserDTO.LastName
            };

            var result = await _userManager.CreateAsync(user, createUserDTO.Password);

            if (!result.Succeeded)
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            if(createUserDTO.Role == null)
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            else if (await _roleManager.RoleExistsAsync(createUserDTO.Role))
                await _userManager.AddToRoleAsync(user, createUserDTO.Role);
        
            await _userRepository.SaveChangesAsync();

            var userDTO = new UserDTO() { 
                Id=user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
            };

            return userDTO;
        }

        public async Task UpdateUserAsync(UserDTO user)
        {
            //_userRepository.UpdateUser(user);
            //await _userRepository.SaveChangesAsync(); // Save changes to the database
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return false;

            _userRepository.RemoveUser(user);
            await _userRepository.SaveChangesAsync(); // Save changes to the database

            return true;
        }
    }
}
