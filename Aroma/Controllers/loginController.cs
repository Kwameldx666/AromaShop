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
        internal ISession _session;
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
        public ActionResult login(LoginData data)
        {

            /*    var uLoginData = new ULoginData
                {
                    IP = "",

                    password = data.Password,
                    credential = data.Username,
                    FirstLoginTime = DateTime.Now

                };*/

            var uLoginData = new ULoginData
            {
                IP = "",

                password = "passwprd",
                credential = "data.Username",
                FirstLoginTime = DateTime.Now
            };
            RResponseData response = _session.UserLoginAction(uLoginData);
           
            if (response != null && response.Status)
            {
             
                    return RedirectToAction("Index", "Home");
                
                
              
       

            }
            return RedirectToAction("login", "home");


        }
    }
}