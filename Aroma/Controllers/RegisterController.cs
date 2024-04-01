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
    public class RegisterController : Controller
    {
        internal ISession _session;
        public RegisterController ()
        {
            var logicBL = new BussinesLogic();
            _session = logicBL.GetSessionBL();

        }
        // GET: Register
        public ActionResult Register()
        {


            return View();
        }
        [HttpPost]

        public ActionResult Register(RegisterData RegData)
        {

            var uRegData = new URegisterData()
            {
                Name = RegData.Username,
                Password = RegData.Password,
                Email = RegData.Email,
                IP = "0.0.0.0",
                RegDate = DateTime.Now,
                AcceptPassword = RegData.AcceptPassword


            };

            URegisterResp uRegisterResp = _session.UserRegisterAction(uRegData);
            if (uRegisterResp != null && uRegisterResp.Status)
            {

                return RedirectToAction("Index", "Home");





            }
            if (uRegisterResp.ResponseMessage != null)
            {
                ViewBag.ErrorPassword = uRegisterResp.ResponseMessage;
            }

            return View("~/Views/Register/Register.cshtml");


        }
    }
    
}
