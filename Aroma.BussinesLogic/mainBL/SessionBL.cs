using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aroma.BussinesLogic.mainBL
{
    public class SessionBL : UserAPI, ISession
    {
        public RResponseData UserLoginAction(ULoginData data)
        {

            return ULASessionCheck(data);
        }


        public URegisterResp UserRegisterAction(URegisterData uRegisterData)
        {
            return RRegisterUpService(uRegisterData);
        }

        public HttpCookie GenCookie(string loginCredential, bool rememberMe)
        {
            return Cookie(loginCredential, rememberMe);
        }

        public UserMinimal GetUserByCookie(string apiCookieValue)
        {
            return UserCookie(apiCookieValue);
        }

        public ResponseLogout UserLogout()
        {
            return LogoutUser();
        }

        public ResponseViewProfile ViewProfile(int userId)
        {
            return ViewProfileAction(userId);
        }

        public ResponseToEditProfile ProfileUpdateAction(ULoginData updateProfile)
        {
            return EditProfileAction(updateProfile);
        }

        public ResponseEditPassword EditUserPass(ULoginData user, string newPassword)
        {
            return EditPassword(user,newPassword);
        }


    }
}
