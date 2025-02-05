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
            var userDTOs = new List<UserDTO>();

            foreach (var user in users)
            {
                
                var userDTO = _mapper.Map<UserDTO>(user);
                await MapRoleDTO(userDTO, user);
                userDTOs.Add(userDTO);
            }

            return userDTOs;
        }

        private async Task MapRoleDTO(UserDTO userDTO, User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userDTO.Roles = roles.ToList();
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            var userDTO = _mapper.Map<UserDTO>(user);
            await MapRoleDTO(userDTO, user);
            return userDTO;
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            var user = _mapper.Map<User>(createUserDTO);

            var result = await _userManager.CreateAsync(user, createUserDTO.Password);

            if (!result.Succeeded)
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            var rolesToAssign = createUserDTO.Roles?.Any() == true ? createUserDTO.Roles : new List<string> { UserRoles.User };

            foreach (var role in rolesToAssign)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    throw new Exception($"Role '{role}' does not exist.");

                await _userManager.AddToRoleAsync(user, role);
            }

            await _userRepository.SaveChangesAsync();

            var userDTO = _mapper.Map<UserDTO>(user);
            await MapRoleDTO(userDTO, user);

            return userDTO;
        }

        public async Task<UserDTO> UpdateUserAsync(string id, UpdateUserDTO updateUserDTO)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            _mapper.Map(updateUserDTO, user);

            // Get the current roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Update roles if provided
            if (updateUserDTO.Roles?.Any() == true)
            {
                // Remove roles no longer in the updated list
                var rolesToRemove = currentRoles.Except(updateUserDTO.Roles).ToList();
                if (rolesToRemove.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeResult.Succeeded)
                        throw new Exception("Failed to remove old roles: " + string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                }

                // Add new roles not already assigned
                var rolesToAdd = updateUserDTO.Roles.Except(currentRoles).ToList();
                foreach (var role in rolesToAdd)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                        throw new Exception($"Role '{role}' does not exist.");

                    var addResult = await _userManager.AddToRoleAsync(user, role);
                    if (!addResult.Succeeded)
                        throw new Exception("Failed to assign roles: " + string.Join(", ", addResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                // Remove all roles
                if (currentRoles.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                        throw new Exception("Failed to remove old roles: " + string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                }
            }

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChangesAsync();

            var userDTO = _mapper.Map<UserDTO>(user);
            await MapRoleDTO(userDTO, user);

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
