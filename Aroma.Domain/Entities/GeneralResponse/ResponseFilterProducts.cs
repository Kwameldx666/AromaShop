using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseFilterProducts
    {
        public bool Success { get; set; }
        public List<ProductDbTable> FilteredProducts { get; set; }
        public string ErrorMessage { get; set; }
    }

}
