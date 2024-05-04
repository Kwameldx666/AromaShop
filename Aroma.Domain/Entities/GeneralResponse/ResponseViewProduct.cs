using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseViewProduct
    {
        public bool Status { get; set; }

        public string Message { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<object> Orders { get; set; }
        public string ImageUrl { get; set; }
        public int View { get; set; }
        public decimal PriceWidthDiscount { get; set; }
        public double AverageRating { get; set; }
        public bool IsPurchased { get; set; }
    }
}
