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
        
            return View();
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

        public ActionResult login()
        {
            return View();
        }

        public ActionResult AddProduct() { return View(); }
    }
}