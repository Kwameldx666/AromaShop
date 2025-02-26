using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Aroma.Helpers
{
    public class USendPasswordResetEmail
    {
        public static bool SendPasswordResetEmail(string email, string newPassword)
        {
            try
            {
                var fromAddress = new MailAddress("artemios.sologan@mail.ru", "Aroma"); // Указываем отправителя (ваш адрес электронной почты и имя отправителя)
                var toAddress = new MailAddress(email); // Указываем получателя (адрес электронной почты получателя)
                const string fromPassword = "hUTMguJHqXvJr3UKn6hn\r\n"; // Укажите ваш пароль от электронной почты отправителя
                const string subject = "Password Reset"; // Укажите тему письма
                string body = "Your new password is: " + newPassword; // Укажите текст сообщения

                var smtp = new SmtpClient
                {
                    Host = "smtp.mail.ru", // Укажите SMTP-сервер, использующийся для отправки почты
                    Port = 587, // Порт SMTP-сервера
                    EnableSsl = true, // Включаем SSL-шифрование для безопасной передачи данных
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message); // Отправляем сообщение
                }

                return true; // Возвращаем true, если письмо успешно отправлено
            }
            catch (Exception ex)
            {
                // Логируем ошибку, если письмо не удалось отправить
                Console.WriteLine("Failed to send email: " + ex.Message);
                return false; // Возвращаем false в случае ошибки
            }
        }

        public static bool SendConfirmationEmail(string emailAddress, string confirmationCode)
        {
            try
            {
                var fromAddress = new MailAddress("wonderful_by@bk.ru", "Aroma"); // Указываем отправителя (ваш адрес электронной почты и имя отправителя)
                var toAddress = new MailAddress(emailAddress); // Указываем получателя (адрес электронной почты получателя)
                const string fromPassword = "N1MtfqLZ4UPnM5eHUnqh\r\n"; // Укажите ваш пароль от электронной почты отправителя
                const string subject = "Accept register"; // Укажите тему письма
                string body = "Your code is: " + confirmationCode; // Укажите текст сообщения

                var smtp = new SmtpClient
                {
                    Host = "smtp.mail.ru", // Укажите SMTP-сервер, использующийся для отправки почты
                    Port = 587, // Порт SMTP-сервера
                    EnableSsl = true, // Включаем SSL-шифрование для безопасной передачи данных
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message); // Отправляем сообщение
                }

                return true; // Возвращаем true, если письмо успешно отправлено
            }
            catch (Exception ex)
            {
                // Логируем ошибку, если письмо не удалось отправить
                Console.WriteLine("Failed to send email: " + ex.Message);
                return false; // Возвращаем false в случае ошибки
            }
        }

    }
}
