using SkillStackCSharp.Services.Interfaces;
using SkillStackCSharp.Repositories.Interfaces;
using SkillStackCSharp.Models;
using SkillStackCSharp.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using SkillStackCSharp.Constants;
using SkillStackCSharp.DTOs.ProductDTOs;
using AutoMapper;

namespace SkillStackCSharp.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository, 
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager,
            IMapper mapper
        ) {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userDTOs;
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            var user = _mapper.Map<User>(createUserDTO);

            var result = await _userManager.CreateAsync(user, createUserDTO.Password);

            if (!result.Succeeded)
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));


            await _userManager.AddToRoleAsync(user, UserRoles.User);

            // to do:
            //if (createUserDTO.Role == null)
            //    await _userManager.AddToRoleAsync(user, UserRoles.User);
            //else if (await _roleManager.RoleExistsAsync(createUserDTO.Role))
            //    await _userManager.AddToRoleAsync(user, createUserDTO.Role);
        
            await _userRepository.SaveChangesAsync();

            var userDTO = _mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public async Task<UserDTO> UpdateUserAsync(string id, UpdateUserDTO updateUserDTO)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;
            _mapper.Map(updateUserDTO, user);
            _userRepository.UpdateUser(user);
            await _userRepository.SaveChangesAsync();
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
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

        public async Task<IdentityResult> ChangePasswordAsync(string id,string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result;
        }
    }
}
