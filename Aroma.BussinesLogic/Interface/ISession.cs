using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Interface
{
    public interface ISession
    {
        RRespoceData UserLoginAction(ULoginData data);
    }
}
