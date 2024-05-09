
using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Aroma.BussinesLogic.Interface
{
    public interface ISession
    {
        HttpCookie GenCookie(string credential, bool rememberMe);
        UserMinimal GetUserByCookie(string value);
        RResponseData UserLoginAction(ULoginData data);
        URegisterResp UserRegisterAction(URegisterData uRegisterData);
        ResponseLogout UserLogout();

        ResponseViewProfile ViewProfile(int userId);
        ResponseToEditProfile ProfileUpdateAction(ULoginData updateProfile);
        ResponseEditPassword EditUserPass(ULoginData user,string newPassword);
        ResponseToEditProfile ForgotPassword(ULoginData updatePassword);

        ResponseCheckCode CheckEmail(string updatePassword, bool editProfile,string email);
    }
}
