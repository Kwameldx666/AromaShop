using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_TW.Models
{
    public class RegisterData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        
        public string Code { get; set; }
        public string AcceptPassword { get; set; }
    }
}