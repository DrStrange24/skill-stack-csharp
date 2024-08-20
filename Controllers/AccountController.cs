using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] User model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                // Hash the password and set it
                user.PasswordHash = _passwordHasher.HashPassword(user, model.PasswordHash);

                // Save the user to the database
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "User created successfully!" });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }
    }

}
