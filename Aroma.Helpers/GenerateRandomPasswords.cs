using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Helpers
{
    public class GenerateRandomPasswords
    {
        public static string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            Random random = new Random();
            int passwordLength = 10; // Длина пароля

            // Генерируем случайный пароль из допустимых символов
            char[] password = new char[passwordLength];
            for (int i = 0; i < password.Length; i++)
            {
                password[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(password);
        }
        public static string GenerateCode()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            int passwordLength = 4; // Длина пароля

            // Генерируем случайный пароль из допустимых символов
            char[] password = new char[passwordLength];
            for (int i = 0; i < password.Length; i++)
            {
                password[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(password);
        }
    }
}
