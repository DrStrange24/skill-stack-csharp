using WebApp.DTOs;
using WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalWebApp.Services.Implementations;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenService _jwtTokenService;

        public AccountController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            JwtTokenService jwtTokenService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                // Save the user to the database
                var result = await _userManager.CreateAsync(user, model.Password);

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

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model, string returnUrl = null)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Find the user by their email or username
            var user = await _userManager.FindByNameAsync(model.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(model.UsernameOrEmail);

            if (user == null) return Unauthorized("Invalid login attempt.");

            // Check if the email is confirmed
            if (!await _userManager.IsEmailConfirmedAsync(user)) return Unauthorized("You need to confirm your email before logging in.");

            // Attempt to sign in the user
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var token = _jwtTokenService.GenerateToken(user);
                return Ok(new { Token = token, Message = "Login successful" });
            }

            if (result.IsLockedOut) return Forbid("User account is locked out.");

            return Unauthorized("Invalid login attempt.");
        }

    }

}
