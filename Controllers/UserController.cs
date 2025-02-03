using SkillStackCSharp.Models;
using SkillStackCSharp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SkillStackCSharp.Constants;
using System.Security.Claims;
using SkillStackCSharp.DTOs.UserDTOs;

namespace SkillStackCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Admin can access all or by the current user
            if (User.IsInRole(UserRoles.Admin) || userId == id)
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                    return NotFound($"User with Id = {id} not found.");

                return Ok(user);
            }

            return Forbid("You can only access your own data.");
        }

        [Authorize(Roles = UserRoles.Admin)]
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
        public async Task<IActionResult> Update(string id, [FromBody] User updatedUser)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "This endpoint is currently under construction");
            if (updatedUser == null)
                return BadRequest("Updated user is null.");

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound($"User with Id = {id} not found.");

            // Update the user properties
            //user.Name = updatedUser.Name;
            //user.Price = updatedUser.Price;

            await _userService.UpdateUserAsync(user);

            return Ok(user);
        }

        // Delete a user by Id
        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task<IActionResult> Delete(string id)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "This endpoint is currently under construction");
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound($"User with Id = {id} not found.");

            await _userService.DeleteUserAsync(user);

            _logger.LogInformation($"Deleted user with Id = {id}");

            return NoContent();
        }
    }
}
