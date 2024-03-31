using Aroma.BussinesLogic.DBModel.Seed;
using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Core.Levels
{
    public class UserAPI
    {

        public RResponseData ULASessionCheck(ULoginData data)

        {

            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == data.credential);
            }

            object ss = null;


                if (data.credential == "login" && data.password == "password")
            {
                return new RResponseData { Status = true };
            }
            return new RResponseData { Status = false };
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
                Level = UserRole.User



            };
            using (var db = new UserContext())
            {
                db.Users.Add(User);
                db.SaveChanges();
            }
            return new URegisterResp { Status = false };
        }
    }
    }
   
