using SkillStackCSharp.DTOs.UserDTOs;
using SkillStackCSharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillStackCSharp.Services.Implementations;
using Microsoft.AspNetCore.Identity.UI.Services;
using SkillStackCSharp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SkillStackCSharp.DTOs.AccountDTOs;
using AutoMapper;

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
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            JwtTokenService jwtTokenService,
            IEmailSender emailSender,
            IConfiguration configuration,
            IUserService userService,
            IMapper mapper
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _emailSender = emailSender;
            _configuration = configuration;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] CreateUserDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userDTO = await _userService.CreateUserAsync(model);

            if (userDTO == null)
                return BadRequest("User creation failed.");

            var user = _mapper.Map<User>(userDTO);
            SendEmailConfirmation(user);
            return Ok(new { Message = "User created successfully! Confirmation email sent." });
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
                var userDTO = _mapper.Map<UserDTO>(user);
                return Ok(new { Token = token.Result, Message = "Login successful", User = new { userDTO.Id } });
            }

            if (result.IsLockedOut) return Forbid("User account is locked out.");

            return Unauthorized("Invalid login attempt.");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest(new { Message = "User ID and token are required." });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = $"Unable to find user with ID '{userId}'." });

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return Ok(new { Message = "Email confirmed successfully!" });

            return BadRequest(new { Message = "Error confirming email.", result.Errors });
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            var updateProfileDTO = _mapper.Map<UpdateUserDTO>(model);
            var user = await _userService.UpdateUserAsync(userId, updateProfileDTO);

            if (user == null)
                return NotFound("User not found.");

            return Ok(new { Message = "Profile updated successfully."});
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            var result = await _userService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Password changed successfully." });
        }


        private async void SendEmailConfirmation(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var baseURL = _configuration["AppSettings:BaseUrl"];
            var confirmationLink = $"{baseURL}/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
            var subject = "SkillStackCSharp Email Confirmation";
            string message = $@"
                <html>
                    <body>
                        <p>Hello {user.FirstName},</p>
                        <p>Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a></p>
                        <p>Thank you for signing up!</p>
                        <p>Regards,</p>
                        <p>SkillStackCSharp</p>
                    </body>
                </html>";
            await _emailSender.SendEmailAsync(user.Email, subject, message);
        }
    }

}
