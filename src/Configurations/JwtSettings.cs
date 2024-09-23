using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Configurations
{
    public class JwtSettings
    {
        public string? Key { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}