using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseCheckCode
    {
        public bool Status { get; set; }    

        public string ErrorMessage { get;set; }
    }
}
