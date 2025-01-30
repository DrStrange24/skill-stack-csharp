using SkillStackCSharp.DTOs;
using SkillStackCSharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillStackCSharp.Services.Implementations;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Net.Mail;
using Azure.Core;

namespace SkillStackCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            JwtTokenService jwtTokenService,
            IEmailSender emailSender
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _emailSender = emailSender;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
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
                SendEmailConfirmation(user);
                return Ok(new { Message = "User created successfully!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model, string returnUrl = null)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Find the user by their email or username
            var user = await _userManager.FindByNameAsync(model.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(model.UsernameOrEmail);

            if (user == null) return Unauthorized("Invalid login attempt.");

            // Check if the email is confirmed
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                SendEmailConfirmation(user);
                return Unauthorized("You need to confirm your email before logging in. Please check your email for confirmation.");
            }

            // Attempt to sign in the user
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var token = _jwtTokenService.GenerateToken(user);
                var userDTO = new UserDTO() { 
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailConfirmed = user.EmailConfirmed,
                };
                return Ok(new { Token = token, Message = "Login successful", User = userDTO });
            }

            if (result.IsLockedOut) return Forbid("User account is locked out.");

            return Unauthorized("Invalid login attempt.");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Message = "User ID and token are required." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = $"Unable to find user with ID '{userId}'." });
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Email confirmed successfully!" });
            }

            return BadRequest(new { Message = "Error confirming email.", Errors = result.Errors });
        }

        private async void SendEmailConfirmation(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token = token }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Personal Web App Email Confirmation", $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Personal Web App</a>");
        }
    }

}
