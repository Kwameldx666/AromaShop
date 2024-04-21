using Aroma.BussinesLogic;
using Aroma.BussinesLogic.Interface;
using Lab_TW.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_TW.Atributes
{
    public class AdminAndModeratorAttribute : ActionFilterAttribute
    {
        public readonly ISession _sessionBusinessLogic;

        public AdminAndModeratorAttribute()
        {
            var businessLogic = new BussinesLogic();
            _sessionBusinessLogic = businessLogic.GetSessionBL();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var apiCookie = HttpContext.Current.Request.Cookies["X-KEY"];

            if (apiCookie != null)
            {
                var profile = _sessionBusinessLogic.GetUserByCookie(apiCookie.Value);
            

                if (profile != null && profile.Level == Aroma.Domain.Entities.User.UserRole.Admin || profile.Level == Aroma.Domain.Entities.User.UserRole.Moderator)
                {
                    HttpContext.Current.SetMySessionObject(profile);
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "Error404" }));
                }
            }

        }
    }
}