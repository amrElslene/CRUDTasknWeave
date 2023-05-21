using CRUDTasknWeave.Models;
using CRUDTasknWeave.Helpers;
using CRUDTasknWeave.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace CRUDTasknWeave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        private string generateJwtToken(ApplicationUser user)
        {
            var jwttokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection(key: "JwtConfig:Key").Value);
            var tokendiscriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(type:"Id",value:user.Id),
                    new Claim(type:JwtRegisteredClaimNames.Sub,value:user.Email),
                    new Claim(type:JwtRegisteredClaimNames.Email,value:user.Email),
                    new Claim(type:JwtRegisteredClaimNames.Jti,value:Guid.NewGuid().ToString()),
                    new Claim(type:JwtRegisteredClaimNames.Iat,value:DateTime.Now.ToUniversalTime().ToString()),

                }),

                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha256),
            };

            var token = jwttokenhandler.CreateToken(tokendiscriptor);
            var jwtToken = jwttokenhandler.WriteToken(token);
            return jwtToken;
        }


        [HttpPost]
        [Route(template: "Register")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Register([FromBody] UserRegisteration model, string role)
        {
            if (ModelState.IsValid)
            {
                // Check if the email is already registered
                var userEx = await _userManager.FindByEmailAsync(model.Email);
                if (userEx != null)
                {
                    return Conflict("Email is already registered.");
                }

                // Create a new user
                ApplicationUser user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                };

                // Hash and salt the password
                string PassHash = PasswordHasher.HashPassword(model.Password);
                // Save the user to the database

                var result = await _userManager.CreateAsync(user, PassHash);
                if (!result.Succeeded)
                {
                    return BadRequest("Invalid user data.");
                }
                // Add role
                if (role.ToLower() == "manager")
                    await _userManager.AddToRoleAsync(user, "Manager");

                else if (role.ToLower() == "user")
                    await _userManager.AddToRoleAsync(user, "User");

                else if (role.ToLower() == "administrator")
                    await _userManager.AddToRoleAsync(user, "Administrator");

                else return BadRequest("Invalid Role");

                string token = generateJwtToken(user);
                return Ok(new AuthResult()
                {
                    Result = true,
                    Token = token
                });
            }

            return BadRequest("Invalid user data.");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false
                });
            }


            if (!PasswordHasher.VerifyPassword(model.Password, user.PasswordHash))
            {
                return BadRequest("Invalid password.");
            }

            var token = generateJwtToken(user);
            return Ok(new AuthResult()
            {
                Token = token,
                Result = true
            });
        }


        [HttpPost]
        [Route(template: "SignOut")]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();

            return Ok("User signed out successfully.");
        }
    }
}


