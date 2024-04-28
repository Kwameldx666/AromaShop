using Aroma.BussinesLogic.Interface;
using Aroma.BussinesLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Support;
using AutoMapper;
using Lab_TW.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.Web.Mvc;

namespace Lab_TW.Controllers
{
    public class SupportController:BaseController
    {
        private readonly ISupport _support;
        // GET: Home
        public SupportController()
        {
            var bl = new BussinesLogic();
            _support = bl.GetSupport();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(SupportForm supportForm)
        {
            int UserId = (int)Convert.ToUInt32(Session["UserId"]);
            if (UserId == 0)
            {
                GetUserId();
                if (UserId == -1)
                {
                    return RedirectToAction("Login", "Account");
                }
                UserId = (int)Convert.ToUInt32(Session["UserId"]);
            }

            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<SupportForm, USupportForm>());

                var Data = Mapper.Map<USupportForm>(supportForm); // Исправлено здесь
                {
                    Data.MessageTime = DateTime.Now;

                };
                ResponseSupport response = _support.SendMessageToSupport(UserId, Data);

            }


            return View();
        }
    }
}