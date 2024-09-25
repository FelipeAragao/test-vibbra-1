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
            return Ok("Welcome!");
        }
    }
}