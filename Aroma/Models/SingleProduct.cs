using Aroma.Domain.Entities.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_TW.Models
{
    public class SingleProduct
    {
        public Product Products { get; set; }
        public List<RatingData> View { get; set; }

       
    }
}