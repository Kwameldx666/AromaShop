using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.Interface ;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.mainBL
{
    public class SupportBL: AdminAPI, ISupport
    {
       public  ResponseSupport SendMessageToSupport(int userId,USupportForm supportForm)
        {
            return MessageToSupportAction(userId,supportForm);
        }

        public ResponseSupport GetViewPort()
        {
            return GetViewPortAction();
        }

        public async Task<ResponseSupport> DeleteUserAction(int userId)
        {
            return await DeleteUserAccountAction(userId);
        }
        public Task<ResponseSupport> GetAdminPanelUsers(int currentUserId)
        {
            return GetAdminPanelUsersAction(currentUserId);
        }
        public Task<ResponseSupport> ChangeUserRole(int userId, string newRole)
        {
            return ChangeUserRoleAction(userId, newRole);
        }
    }
}
