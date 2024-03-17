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
            if (data.credential == "login" && data.password == "password")
            {
                return new RResponseData { Status = true };
            }
            return new RResponseData { Status = false };
        }
    }
    }
   
