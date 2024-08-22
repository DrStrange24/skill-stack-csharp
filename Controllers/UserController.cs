﻿using PersonalWebApp.Models;
using PersonalWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PersonalWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        // Get all users
        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Get a user by Id
        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound($"User with Id = {id} not found.");

            return Ok(user);
        }

        // Create a new user (already provided but updated to async)
        [HttpPost(Name = "PostUser")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (user == null)
                return BadRequest("User is null.");

            await _userService.CreateUserAsync(user);

            //_logger.LogInformation($"Created user: {user.Name}, Price: {user.Price}");

            return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
        }

        // Update an existing user
        [HttpPut("{id:int}", Name = "UpdateUser")]
        public async Task<IActionResult> Update(int id, [FromBody] User updatedUser)
        {
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
        [HttpDelete("{id:int}", Name = "DeleteUser")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound($"User with Id = {id} not found.");

            await _userService.DeleteUserAsync(user);

            _logger.LogInformation($"Deleted user with Id = {id}");

            return NoContent();
        }
    }
}
