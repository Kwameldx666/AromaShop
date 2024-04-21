using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseViewProfile
    {
        public bool Status {  get; set; }
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public decimal Balance { get; set; }
    }
}
