using Aroma.BussinesLogic.DBModel.Seed;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Core.Levels
{
    public class AdminAPI
    {


  

        public ResponseAddProduct AddAdminActionProduct(Product products)
        {
            if (products == null) 
            {
                string Error = "Error to add product";
                return new ResponseAddProduct { Status = false ,MessageError = Error};
            }

            var product = new ProductDbTable()
            {
                Name = products.Name,
                Price = products.Price,
                ProductType = products.ProductType,
                Category = products.Category,
                Description = products.Description,
                Id = products.Id
            };
            using(var db = new ProductContext())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
            return new ResponseAddProduct { Status = true };
        }

        
    }
}
