using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_TW.Models
{
    public class AdminPanelForUserData
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        // Конструктор для инициализации объекта
        public AdminPanelForUserData(int userId, string username, string email, string role)
        {
            UserId = userId;
            Username = username;
            Email = email;
            Role = role;
        }
    }

}