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
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticateController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly JwtSettings _jwtSettings;

        public AuthenticateController(ILoginService loginService, IOptions<JwtSettings> jwtSettings)
        {
            this._loginService = loginService;
            this._jwtSettings = jwtSettings.Value;
        }

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

        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest(new { error = "Invalid login data" });
            }

            var userDTO = await this._loginService.Login(loginDTO);
            if (userDTO != null)
            {
                var claims = new List<Claim>() {
                    new Claim("Login", userDTO.Login),
                    new Claim("Email", userDTO.Email)
                };
                string token = GenerateJwtToken(claims);
                return Ok(new {
                    token = token,
                    user = userDTO
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
            /*
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (authenticateResult.Succeeded)
            {
                // Authenticated user yet, return token
                var userClaims = authenticateResult.Principal.Claims;
                var token = GenerateJwtToken(userClaims);

                return Ok(new { token });
            }
*/
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
                    // User's authenticated data
                    var userClaims = result.Principal.Claims;

                    var token = GenerateJwtToken(userClaims);
                    return Ok(new { token });
                }
                return Unauthorized(new { error = "Unauthorized" });
            }
            catch (Exception ex)
            {
                // Log do erro e detalhes
                Console.WriteLine(ex.Message);
                return BadRequest(new { error = "An error occurred during the Google login process." });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}