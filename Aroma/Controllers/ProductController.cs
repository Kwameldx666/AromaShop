using Aroma.BussinesLogic;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_TW.Controllers
{
    public class ProductController : Controller
    {
      
     

        internal IProduct _product;
        internal ISession _session;
        public ProductController()
        {
            var bl = new BussinesLogic();
            _product = bl.AddProductBL();
         
        }

        [HttpPost]
        public ActionResult Index(Product product) 
        {
            var AdminAddProduct = new Product
            {
                Price = product.Price,
                Id = product.Id,
                Description = product.Description,
                Name = product.Name,
                Category = product.Category
                
                
            };
            RResponseData response = _product.AdminAddAction(AdminAddProduct);
            {
                if (response !=null && response.Status)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                    return RedirectToAction("AddProduct", "Home");
            }
    
        }
    }
}