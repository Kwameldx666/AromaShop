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
        [HttpGet]
        public ActionResult AddProducts()
        {
            // Инициализируем модель с одним пустым продуктом для отображения на форме
            var products = new List<Product> { new Product() };
            return View(products);
        }
        [HttpPost]

        public ActionResult Index(List<Product> products)
        {
            if (products != null && products.Count > 0)
            {
                foreach (var product in products)
                {
                    var AdminAddProduct = new Product
                    {
                        Price = product.Price,
                        Id = product.Id,
                        Description = product.Description,
                        Name = product.Name,
                        Category = product.Category,
                        Count = products.Count,
                 
                        // Убедитесь, что у вас есть соответствующие свойства в вашем классе Product
                    };


                    RResponseData response = _product.AdminAddAction(products);
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
            return View(products);
        }
    }
}
