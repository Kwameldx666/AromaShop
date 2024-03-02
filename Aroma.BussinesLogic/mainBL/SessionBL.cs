using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.mainBL
{
    public class SessionBL:UserAPI, ISession
    {
        public RRespoceData UserLoginAction (ULoginData data)
        {
            return ULASessionCheck(data);
        }
        
    }
}
