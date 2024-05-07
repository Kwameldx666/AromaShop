using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aroma.BussinesLogic.Interface
{
    public interface ISupport
    {
        Task<ResponseSupport> ChangeUserRole(int userId, string newRole);
        Task<ResponseSupport> DeleteUserAction(int userId);
        Task<ResponseSupport> GetAdminPanelUsers(int currentUserId);
        ResponseSupport GetViewPort();
        ResponseSupport SendMessageToSupport(int userId,USupportForm supportForm);
    }
}

