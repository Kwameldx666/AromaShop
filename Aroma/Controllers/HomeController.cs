using Aroma.BussinesLogic;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponse;
using Lab_TW.Extension;
using Lab_TW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Lab_TW.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProduct _product;
        // GET: Home
        public HomeController()
        {
            var bl = new BussinesLogic();
            _product = bl.AddProductBL();
        }
        public ActionResult Index()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.IsUserLoggedIn = true;
            var user = System.Web.HttpContext.Current.GetMySessionObject();

            ResponseGetProducts response = _product.AdminGetAction();

            var viewModelProducts = response.Products.Select(p => new Lab_TW.Models.Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                ProductType = p.ProductType,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Quantity = p.Quantity,
            }).ToList();
            if (response.Status)
            {
                // Если запрос прошёл успешно, отображаем список продуктов
                return View(viewModelProducts);
            }
            else
            {
                // Если при запросе возникла ошибка, отображаем сообщение об ошибке
                ViewBag.ErrorMessage = response.Message;
                return View("Error");
            }

 

        }




        public ActionResult Blog()
        {
            return View();
        }


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