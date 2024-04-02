using Aroma.BussinesLogic.DBModel.Seed;
using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Aroma.Helpers;

namespace Aroma.BussinesLogic.Core.Levels
{
    public class UserAPI
    {

        public RResponseData ULASessionCheck(ULoginData data)

        {

            using (var db = new UserContext())
            {
                // Поиск пользователя в базе данных по имени пользователя
                var user = db.Users.FirstOrDefault(u => u.Username == data.credential);
                if (user == null)
                {
                    user = db.Users.FirstOrDefault(u => u.Email == data.credential);
                }
                string pass = LoginHelper.HashGen(data.password);
                if (user != null && user.Password == pass)
                {
                    // Аутентификация успешна
                    return new RResponseData { Status = true };
                }

                // Если пользователь не найден или пароль не совпадает
                return new RResponseData { Status = false };
            }
        }

        public URegisterResp RRegisterUpService (URegisterData data)
        {
            var User = new UDbTable
            {
                Username = data.Name,
                Password = data.Password,
                LastIP = data.IP,
                Email = data.Email,
                LastLogin = data.RegDate,
                Level = UserRole.User,
             


            };


                
                    if (User.Password != data.AcceptPassword)
                    {
                        string errorPassword = "Пароли не совпадают";
                        return new URegisterResp { Status = false, ResponseMessage = errorPassword };
                    }


            User.Password = LoginHelper.HashGen(User.Password);
            

            using (var db = new UserContext())
            {
                db.Users.Add(User);
                db.SaveChanges();
            }

            return new URegisterResp { Status = true };
        }


        internal HttpCookie Cookie(string loginCredential)
        {
            var apiCookie = new HttpCookie("X-KEY")
            {
                Value = CookieGenerator.Create(loginCredential)
            };

            using (var db = new SessionContext())
            {
                Session curent;
                var validate = new EmailAddressAttribute();
                if (validate.IsValid(loginCredential))
                {
                    curent = (from e in db.Sessions where e.Username == loginCredential select e).FirstOrDefault();
                }
                else
                {
                    curent = (from e in db.Sessions where e.Username == loginCredential select e).FirstOrDefault();
                }

                if (curent != null)
                {
                    curent.CookieString = apiCookie.Value;
                    curent.ExpireTime = DateTime.Now.AddMinutes(60);
                    using (var todo = new SessionContext())
                    {
                        todo.Entry(curent).State = EntityState.Modified;
                        todo.SaveChanges();
                    }
                }
                else
                {
                    db.Sessions.Add(new Session
                    {
                        Username = loginCredential,
                        CookieString = apiCookie.Value,
                        ExpireTime = DateTime.Now.AddMinutes(60)
                    });
                    db.SaveChanges();
                }
            }

            return apiCookie;
        }

    }
    }
   
