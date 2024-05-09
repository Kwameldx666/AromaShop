using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseToEditProfile
    {
        public bool Status { get; set; }

        public bool ChangeEmail { get; set; }
        public string MessageError {  get; set; }

        public string Email { get; set; }
    }
}
