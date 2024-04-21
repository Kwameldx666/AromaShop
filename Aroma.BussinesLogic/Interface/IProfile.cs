using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Interface
{
    public interface IProfile
    {
        ResponseViewProfile ViewProfile(int userId);
        ResponseToEditProfile ProfileUpdateAction(ULoginData updateProfile);
      
    }
}
