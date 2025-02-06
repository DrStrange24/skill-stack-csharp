using SkillStackCSharp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SkillStackCSharp.Constants;
using SkillStackCSharp.DTOs.UserDTOs;

namespace SkillStackCSharp.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound($"User with Id = {id} not found.");

            return Ok(user);
        }

        [HttpPost(Name = "PostUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO userDto)
        {
            if (userDto == null)
                return BadRequest("User is null.");

            var user = await _userService.CreateUserAsync(userDto);

            _logger.LogInformation($"Created user: {user.UserName}, Email: {user.Email}");

            return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
        }

        // Update an existing user
        [HttpPut("{id}", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUserById(string id, [FromBody] UpdateUserDTO updatedUser)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID cannot be null or empty.");

            if (updatedUser == null)
                return BadRequest("Updated user is null.");

            var result = await _userService.UpdateUserAsync(id, updatedUser);

            if (result == null)
                return NotFound($"User with Id = {id} not found.");

            return Ok(result);
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID cannot be null or empty.");

            var result = await _userService.DeleteUserAsync(id);

            if (!result)
                return NotFound($"User with Id = {id} not found.");

            _logger.LogInformation($"Deleted user with Id = {id}");

            return NoContent();
        }
    }
}
