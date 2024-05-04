using Aroma.Domain.Entities.Rating;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseGetRating
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public ResponseGetRating()
        {
            View = new List<RatingUdbTable>();

        }

        public List<RatingUdbTable> View { get; set; }
        public bool Good { get; set; }


    
    }
}
