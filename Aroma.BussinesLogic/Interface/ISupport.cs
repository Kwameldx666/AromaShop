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
        ResponseSupport SendMessageToSupport(int userId,USupportForm supportForm);
    }
}

