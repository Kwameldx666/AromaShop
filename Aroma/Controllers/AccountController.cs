using Aroma.BussinesLogic;
using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.DBModel.Seed;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.User;
using AutoMapper;
using Lab_TW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tensorflow.Keras.Engine;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static Tensorflow.SummaryMetadata.Types;


namespace Lab_TW.Controllers
{
    public class AccountController : BaseController
    {
        private ISession _session;

        public AccountController()
        {
            var logicBL = new BussinesLogic();
            _session = logicBL.GetSessionBL();

        }

        //Выход с аккаунта
        public ActionResult Logout()
        {

            // Вызываем метод выхода из системы
            ResponseLogout logoutResponse = _session.UserLogout();

            if (logoutResponse.Status)
            {
                // Если выход успешен, можно очистить какие-либо данные сессии или выполнить другие действия
                // Например, удалить куки сессии (если они используются)
                if (Response.Cookies["X-KEY"] != null)
                {
                    var IsUserLoggedIn = false; // Статус аутентификации сохраняется между запросами   
                    var admin = false;
                    var moderator = false;
                    var userId = 0;
                    Session["UserId"] = userId;
                    Session["admin"] = admin.ToString(); // Сохраняем роль в сессии
                    Session["moderator"] = moderator.ToString(); // Сохраняем роль в сессии
                    Session["IsUserLoggedIn"] = IsUserLoggedIn.ToString(); // Сохраняем роль в сессии

                    var cookie = new HttpCookie("X-KEY")
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    Response.Cookies.Add(cookie);
                }

                // Перенаправляем пользователя на страницу входа
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Обработка ошибки выхода, если потребуется
                return RedirectToAction("Index", "Home");
            }
        }
/*    <!-- ============================================================== -->
      <!--                    Авторизация                                 -->
      <!-- ============================================================== -->*/

        public ActionResult login()
        {
            GetUserId(); // Вызов метода для получения идентификатора пользователя
            SessionStatus(); // Проверка статуса сеанса

            return View(); // Возврат представления
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(LoginData data)
        {
            // Проверяем, прошла ли модель валидацию
            if (ModelState.IsValid)
            {
                // Инициализируем маппер AutoMapper для преобразования объекта LoginData в объект ULoginData
                Mapper.Initialize(cfg => cfg.CreateMap<LoginData, ULoginData>());

                // Преобразуем объект LoginData в объект ULoginData
                var Data = Mapper.Map<ULoginData>(data); // Исправлено здесь
                {
                    // Заполняем дополнительные данные, такие как IP-адрес пользователя и время первого входа
                    Data.IP = Request.UserHostAddress;
                    Data.FirstLoginTime = DateTime.Now;
                    Data.credential = data.Username;
                };

                // Вызываем метод для аутентификации пользователя
                RResponseData response = _session.UserLoginAction(Data);

                // Проверяем, успешна ли была аутентификация
                if (response != null && response.Status)
                {
                    // Генерируем и добавляем cookie для пользователя
                    HttpCookie cookie = _session.GenCookie(Data.credential, data.RememberMe);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    // Устанавливаем имя пользователя в ViewBag
                    ViewBag.UserName = Data.credential;

                    // Устанавливаем статус пользователя в сессию
                    Session["IsUserLoggedIn"] = System.Web.HttpContext.Current.Session["LoginStatus"];
                    string f = (string)Session["IsUserLoggedIn"];

                    // Проверяем, является ли пользователь администратором или модератором
                    if (response.AdminMod || response.ModeratorMod)
                    {
                        // Перенаправляем администратора или модератора на соответствующую страницу
                        ViewBag.UserName = Data.credential;
                        return RedirectToAction("ProductsAdminPanel", "Product");
                    }

                    // Перенаправляем пользователя на главную страницу
                    return RedirectToAction("Index", "Home");
                }

                // Если есть сообщение об ошибке, передаем его в ViewBag
                if (response.ResponseMessage != null)
                {
                    ViewBag.ErrorMessage = response.ResponseMessage;
                }

                // Возвращаем пользователя на страницу входа с сообщением об ошибке
                return View("~/Views/Account/Login.cshtml");
            }

            // Возвращаем пользователя на страницу входа, если модель не прошла валидацию
            return View();
        }
/*   <!-- ============================================================== -->
     <!--                    Регистрация                                 -->
     <!-- ============================================================== -->*/
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
                IP = Request.UserHostAddress,
                RegDate = DateTime.Now,
                AcceptPassword = RegData.AcceptPassword

            };
            TempData["Email"] = RegData.Email;
            URegisterResp uRegisterResp = _session.UserRegisterAction(uRegData);
            if (uRegisterResp != null && uRegisterResp.Status)
            {

                return RedirectToAction("ConfirmRegistrationCode");

            }
            if (uRegisterResp.ResponseMessage != null)
            {
                ViewBag.ErrorPassword = uRegisterResp.ResponseMessage;
            }

            return View("~/Views/Account/Register.cshtml");


        }

        [HttpGet]

        public ActionResult ConfirmRegistrationCode()
        {
            return View();

        }

        [HttpPost]

        public ActionResult ConfirmRegistrationCode(string code)
        {/*
            Mapper.Initialize(cfg => cfg.CreateMap<RegisterData, URegisterData>());

            var updatePassword = Mapper.Map<URegisterData>(code);*/
            bool editProfile = false; // Или false в зависимости от условий вашего приложения
            TempData["EditProfile"] = editProfile;
            string email = (string)TempData["Email"];

            {
                editProfile = true;
            };
            ResponseCheckCode response = _session.CheckEmail(code, editProfile, email);
            if (response.Status)
            {
                if (response.Regiser)
                    return RedirectToAction("Login");
                else
                    return RedirectToAction("UProfile");
            }

            if (response.Regiser)
                return RedirectToAction("Login");
            else
                return RedirectToAction("UProfile");
        }

/*   <!-- ============================================================== -->
     <!--                    Изменить все данные профиля                 -->
     <!-- ============================================================== -->*/
        public ActionResult EditProfile(LoginData data)
        {
            GetUserId();


            int UserId = (int)Convert.ToUInt32(Session["UserId"]);
            /*    GetUserId();*/

            Mapper.Initialize(cfg => cfg.CreateMap<LoginData, ULoginData>());

            var updateProfile = Mapper.Map<ULoginData>(data);
            {
                updateProfile.credential = data.Username;
                updateProfile.Id = UserId;
            }


            // Вызов метода бизнес-логики для обновления продукта
            ResponseToEditProfile response = _session.ProfileUpdateAction(updateProfile);

            if (response.Status)
            {
                if (response.ChangeEmail == true)
                {
                    TempData["Email"] = response.Email;

                    return RedirectToAction("ConfirmRegistrationCode");
                }
                bool editProfile = true; // Или false в зависимости от условий вашего приложения

                TempData["EditProfile"] = editProfile;
                return RedirectToAction("UProfile");

            }
            else
            {
                // Если при обновлении произошла ошибка, отображаем сообщение об ошибке
                ViewBag.ErrorMessage = response.MessageError;
                return View("Error"); // Предполагается, что у вас есть представление Error.cshtml для отображения ошибок
            }




        }

 /*   <!-- ============================================================== -->
      <!--                    Изменить пароль                             -->
      <!-- ============================================================== -->*/
        [HttpPost]
        public ActionResult ChangePassword(LoginData user, string newPassword, string confirmPassword)
        {

            // Логика обновления пароля
            int UserId = (int)Convert.ToUInt32(Session["UserId"]);
            if (newPassword == confirmPassword)
            {
                ULoginData NewPassword = new ULoginData()
                {
                    password = newPassword,
                    Id = UserId

                };

                ResponseEditPassword newPass = _session.EditUserPass(NewPassword, newPassword);


                // Возвращаем результат (например, перенаправление на профиль пользователя)
                return RedirectToAction("UProfile");
            }
            else
            {
                // Обработка ошибки, если пароли не совпадают
                ModelState.AddModelError("", "Пароли не совпадают");
                return View("UProfile"); // Перенаправляем обратно на профиль пользователя с ошибкой
            }
        }

 /*   <!-- ============================================================== -->
      <!--                    Забыли пароль                               -->
      <!-- ============================================================== -->*/

       
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Account");
            }


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


            // Используем сервис для получения данных о пользователе по идентификатору
            ResponseViewProfile userProfile = _session.ViewProfile(UserId);

            LoginData data = new LoginData()
            {
                Username = userProfile.Username,
                Email = userProfile.Email,
                Balance = userProfile.Balance

            };



         
                        // Возвращаем представление с информацией о пользователе
                        return View(data);
                
                
               
            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(LoginData data)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<LoginData, ULoginData>());

                var Data = Mapper.Map<ULoginData>(data); // Исправлено здесь
                {
                    Data.IP = Request.UserHostAddress;
                    Data.FirstLoginTime = DateTime.Now;
                    Data.credential = data.Username;
                };

                RResponseData response = _session.UserLoginAction(Data);

                if (response != null && response.Status)
                {
                    
                    HttpCookie cookie = _session.GenCookie(Data.credential, data.RememberMe);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                    ViewBag.UserName = Data.credential;
                    Session["IsUserLoggedIn"] = System.Web.HttpContext.Current.Session["LoginStatus"];
                    string f = (string)Session["IsUserLoggedIn"];
                    ChechEmail(Data.Email);
                    if (response.AdminMod || response.ModeratorMod)
                    {
                        ViewBag.UserName = Data.credential;
                        return RedirectToAction("ProductsAdminPanel", "Product");
                    }
                    return RedirectToAction("Index", "Home");
                }
                if (response.ResponseMessage != null)
                {
                    ViewBag.ErrorMessage = response.ResponseMessage;
                }
                // Возврат на страницу входа с сообщением об ошибке
                return View("~/Views/Account/Login.cshtml");
            }
            // Возврат на страницу входа, если модель не валидна
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(LoginData model)
        {
            if (ModelState.IsValid)
            {

                Mapper.Initialize(cfg => cfg.CreateMap<LoginData, ULoginData>());

                var updatePassword = Mapper.Map<ULoginData>(model);
                ResponseToEditProfile forgotPassword = _session.ForgotPassword(updatePassword);
                if (forgotPassword.Status)
                {
                    // После успешной отправки перенаправьте пользователя на страницу с сообщением об успешной отправке нового пароля
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                else
                {
                    // Если возникла ошибка при отправке email, верните представление с сообщением об ошибке
                    ViewBag.ErrorMessage = "Failed to send password reset email. Please try again later.";
                    return View(model);
                }
            }

            // Если модель не валидна, верните представление с сообщениями об ошибках
            return View(model);
        }


        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
 /*   <!-- ============================================================== -->
      <!--                    Просмотр профиля                            -->
      <!-- ============================================================== -->*/
        [HttpGet]
        public ActionResult UProfile()
        {


            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Account");
            }


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


            // Используем сервис для получения данных о пользователе по идентификатору
            ResponseViewProfile userProfile = _session.ViewProfile(UserId);

            LoginData data = new LoginData()
            {
                Username = userProfile.Username,
                Email = userProfile.Email,
                Balance = userProfile.Balance

            };




            // Возвращаем представление с информацией о пользователе
            return View(data);




        }







    }
        
}

