using Aroma.BussinesLogic;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
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

        public ActionResult AddProduct()
        {
            // Инициализация списка продуктов. Если нет продуктов для отображения,
            // передается пустой список, а не null.
      

            // Здесь может быть логика для заполнения списка продуктов,
            // если в вашем приложении это предусмотрено.

            return View(); // Передача списка в представление.
        }

        [HttpPost]

        public ActionResult AddProducts(Product product)
        {    
                    var AdminAddProduct = new Product
                    {
                        
                        Price = product.Price,
                       
                        Description = product.Description,
                        Name = product.Name,
                        Category = product.Category,
                        ProductType = product.ProductType,
                     


                       
                    };


                    RResponseData response = _product.AdminAddAction(product);
                    {
                        if (response != null && response.Status)
                        {
                            return RedirectToAction("Blog", "Home");

                        }
                        else
                            return RedirectToAction("AddProduct", "Home");
                    }
        }
    }
}
