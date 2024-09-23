using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers.v1
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult LoginGoogle()
        {
            return Ok("Hello world");
        }
/*
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

                    var token = "abc"; //GenerateJwtToken(userClaims);
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
        }*/
    }
}