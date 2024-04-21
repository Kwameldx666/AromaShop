using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_TW.Models
{
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RememberMe { get; set; }

        public string Email {  get; set; }

        public int Id { get; set; }
        public decimal Balance { get; set; }
    }
}