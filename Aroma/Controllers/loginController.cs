using Aroma.BussinesLogic;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.User;
using Lab_TW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            var uLoginData = new ULoginData
            {
                IP = "",
                password = data.Password,
                credential = data.Username,
                FirstLoginTime = DateTime.Now
            };

            RRespoceData responce = _session.UserLoginAction(uLoginData);
            if(responce!= null && responce.Status)
            {
                //logig BD


            }
            return View();
        }
    }
}