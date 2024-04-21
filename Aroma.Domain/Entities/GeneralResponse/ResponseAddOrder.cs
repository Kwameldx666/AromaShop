using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseAddOrder
    {
        public bool Success { get; set; }
        public string MessageError {  get; set; }
        public Order Order { get; set; }
    }
}
