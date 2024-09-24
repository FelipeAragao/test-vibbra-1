using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Configurations
{
    public class CorreiosSettings
    {
        public string? ApiUrl { get; set; }
        public string? NumeroCartaoPostagem { get; set; }
        public int TimeToExpiryTokenInMinutes { get; set; }
        public string? RoutesAutenticaCartaoPostagem { get; set; }
        public string? RoutesPrecoNacional { get; set; }
    }
}