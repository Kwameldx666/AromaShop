﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_TW.Controllers
{

    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Blog()
        {
            return View();
        }
    }
}