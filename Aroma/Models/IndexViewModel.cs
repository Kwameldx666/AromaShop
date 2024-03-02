using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_TW.Models
{
    public class IndexViewModel
    {
        public List<Product> Products { get; set; }
        public List<Product> BestSellers { get; set; }

        public List<NewInfo> News { get; set; }
    }

}