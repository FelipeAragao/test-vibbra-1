using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Configurations;

namespace src.Controllers.v1
{
    // Controllers for Authentication 
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticateController : Controller
    {
        private readonly IUserService _userService;
        private readonly JwtSettings _jwtSettings;

        public AuthenticateController(IUserService userService, IOptions<JwtSettings> jwtSettings)
        {
            this._userService = userService;
            this._jwtSettings = jwtSettings.Value;
        }

        // Centralize the Token's generation
        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var key = this._jwtSettings.Key;
            if(string.IsNullOrEmpty(key))
                return string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(this._jwtSettings.ExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest(new { error = "Invalid login data" });
            }

            var userDTO = await this._userService.Login(loginDTO);
            if (userDTO != null)
            {
                var claims = new List<Claim>() {
                    new Claim("Login", userDTO.Login),
                    new Claim("Email", userDTO.Email)
                };
                string token = GenerateJwtToken(claims);
                return Ok(new TokenDTO {
                    Token = token,
                    User = userDTO
                });
            }
            else
            {
                return Unauthorized(new { error = "Unauthorized" });
            }
        }

        [HttpGet("sso")]
        public IActionResult LoginGoogle()
        {
            // Redirect to Google Callback
            var redirectUrl = Url.Action("LoginGoogleCallback", "Authenticate");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("signin-google")]
        public async Task<IActionResult> LoginGoogleCallback()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (result?.Succeeded == true)
                {
                    var userClaims = result.Principal.Claims;
                    var nameClaim = User.FindFirst(ClaimTypes.Name)?.Value;
                    var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

                    if (nameClaim == null || emailClaim == null)
                    {
                        return Unauthorized(new { error = "Unauthorized. Invalid name or e-mail." });
                    }

                    // Get or add the user on SSO logins (all users must be registered)
                    var userDTO = await this._userService.GetByLogin(emailClaim);
                    if (userDTO == null)
                    {
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%¨&*()-=_+,.<>?|[]{}";
                        var random = new Random();
                        string password = new string(Enumerable.Repeat(chars, 10)
                            .Select(s => s[random.Next(s.Length)]).ToArray());

                        userDTO = new UserDTO() {
                            Name = nameClaim,
                            Email = emailClaim,
                            Login = emailClaim,
                            Password = password
                        };

                        await this._userService.Add(userDTO);
                    }

                    var claims = new List<Claim>() {
                        new Claim("Login", userDTO.Login),
                        new Claim("Email", userDTO.Email)
                    };
                    string token = GenerateJwtToken(claims);
                    return Ok(new TokenDTO {
                        Token = token,
                        User = userDTO
                    });
                }
                return Unauthorized(new { error = "Unauthorized" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "An error occurred during the Google login process. " + ex.Message });
            }
        }
    }
}