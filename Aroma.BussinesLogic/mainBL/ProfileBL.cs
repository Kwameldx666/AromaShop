using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.mainBL
{
    internal class ProfileBL:UserAPI , IProfile
    {
        public ResponseViewProfile ViewProfile(int userId)
        {
            return ViewProfileAction(userId);
        }

        public  ResponseToEditProfile ProfileUpdateAction(ULoginData updateProfile)
        {
            return EditProfileAction(updateProfile);
        }
    }
}
