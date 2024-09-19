using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Application.DTOs
{
    public class LoginDTO
    {
        public required string Login { get; set; }

        public required string Password { get; set; }
    }
}