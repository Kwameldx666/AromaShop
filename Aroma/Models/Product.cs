using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_TW.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string ProductType { get; set; }
        public string Image { get; set; }

    }
}