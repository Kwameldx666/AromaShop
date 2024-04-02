using Aroma.BussinesLogic;
using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.Interface;

using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.User;
using Lab_TW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Tensorflow.SummaryMetadata.Types;

namespace Lab_TW.Controllers
{
    public class loginController : Controller
    {
        private readonly  ISession _session;
        public loginController() 
        {
            var logicBL = new BussinesLogic();
            _session = logicBL.GetSessionBL();

        }
        // GET: login
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(LoginData data)
        {
            if (ModelState.IsValid)
            {
            /*    Mapper.Initialize(cfg => cfg.CreateMap<LoginData, ULoginData>());
                var data = Mapper.Map<ULoginData>(login);*/
                var uLoginData = new ULoginData
                {
                    IP = Request.UserHostAddress,

                    password = data.Password,
                    credential = data.Username,
                    FirstLoginTime = DateTime.Now

                };


                RResponseData response = _session.UserLoginAction(uLoginData);

                if (response != null && response.Status)
                {
                    ViewBag.UserName = uLoginData.credential;
                    ViewBag.IsUserLoggedIn = true; // Статус аутентификации сохраняется между запросами
                   
                    
                    
                    HttpCookie cookie = _session.GenCookie(uLoginData.credential);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                    return View("~/Views/Home/Index.cshtml");





                }
                return RedirectToAction("login", "home");
            }
            return View();
        }
       
    }
}