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
using Lab_TW.Atributes;
using System.Threading.Tasks;

namespace Lab_TW.Controllers
{
    public class SupportController : BaseController
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
        [AdminAndModerator]

        public ActionResult ViewPort()
        {

            ResponseSupport responseSupport = _support.GetViewPort();

            if (responseSupport.Status)
            {

                return View(responseSupport.SupportMesages);
            }
            else
            {
                // Если возникла ошибка, возвращаем представление с сообщением об ошибке
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
            {
                // Ваш код здесь для удаления пользователя
                // Например, вы можете использовать сервис или репозиторий для выполнения этой операции
                ResponseSupport response = await _support.DeleteUserAction(userId);

                if (response.Status)
                {
                    return Json(new { success = true }); // Возвращаем успешный результат в формате JSON
                }
                else
                {
                    return Json(new { success = false, errorMessage = response.StatusMessage }); // Возвращаем ошибку в формате JSON
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, если необходимо
                // Здесь можно добавить логику для записи информации об ошибке
                return Json(new { success = false, errorMessage = ex.Message }); // Возвращаем ошибку в формате JSON
            }
        }

        [AdminMode]
        public async Task<ActionResult> AdminPanelAboutUsers()
        {
            SessionStatus();
            int currentUserId = GetUserId();
            ResponseSupport responseSupport = await _support.GetAdminPanelUsers( currentUserId);
            if (responseSupport.Status) 
            {
                return View(responseSupport.TotalUsers);
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangeUserRoleAction(int userId, string newRole)
        {
            ResponseSupport response = await _support.ChangeUserRole(userId, newRole);

            if (response.Status)
            {
                return Json(new { success = true }); // Возвращаем результат в формате JSON
            }
            else
            {
                return Json(new { success = false, errorMessage = response.StatusMessage });
            }
        }
    
    }
}