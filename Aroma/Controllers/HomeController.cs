using Lab_TW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Lab_TW.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // Создаем экземпляр модели представления
            var viewModel = new IndexViewModel
            {
                // Инициализация списка продуктов
                Products = new List<Product>
        {
            // Здесь вы добавляете продукты, которые будут отображаться на странице
             new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product1.png",ProductType ="Accessories" },
             new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product2.png" ,ProductType ="Beauty" },
             new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product3.png" ,ProductType ="Decor" },
             new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product4.png" ,ProductType ="Accessories" },
             new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product5.png",ProductType ="Accessories" },
             new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product6.png",ProductType ="Accessories" },
             new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product7.png" ,ProductType ="Accessories" },
             new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product8.png", ProductType = "Accessories" }
                         // Добавьте другие продукты по аналогии
        },
                // Инициализация списка лучших продавцов
                BestSellers = new List<Product>
        {
            // Здесь вы добавляете продукты, которые являются лучшими продавцами
          new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product1.png",ProductType ="Accessories" },
          new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product2.png" ,ProductType ="Beauty" },
          new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product3.png" ,ProductType ="Decor" },
          new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product4.png" ,ProductType ="Accessories" },
          new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product5.png",ProductType ="Accessories" },
          new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product6.png",ProductType ="Accessories" },
          new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product7.png" ,ProductType ="Accessories" },
          new Product { Id = 1, Name = "Quartz Belt Watch", Price = 150, Image = "product8.png", ProductType = "Accessories" }

        },
                News = new List<NewInfo>
                {
                    new NewInfo { Id = 1,Name ="The Richland Center Shooping News and weekly shooper",}
                    }
            };

            // Передаем модель представления в представление
            return View(viewModel);
        }




        public ActionResult Blog()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
  //      public ActionResult Index(LoginViewModel login)
   //     {
     //       if (ModelState.IsValid)
    //        {

    //        }

    //        return View();
   //     }
        public ActionResult cart()
        {
            return View();
        }

        public ActionResult category()
        {
            return View();
        }
        public ActionResult checkout()
        {
            return View();
        }
        public ActionResult confirmation()
        {
            return View();
        }

        public ActionResult contact()
        {
            return View();
        }

        public ActionResult register()
        {
            return View();
        }
        public ActionResult SingleBlog()
        {
            return View();
        }
        public ActionResult SingleProduct()
        {
            return View();
        }
        public ActionResult TrackingOrder()
        {
            return View();
        }
    


    }
}