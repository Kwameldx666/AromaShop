using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Lab_TW.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/vendors/bootstrap/bootstrap.min.css",
                        "~/vendors/bootstrap/bootstrap.min.css",
                        "~/css/style.css",
                        "~/vendors/owl-carousel/owl.carousel.min.css",
                        "~/vendors/owl-carousel/owl.theme.default.min.css",
                        "~/vendors/style.css",
                        "~/vendors/themify-icons/themify-icons.css",
                        "~/vendors/fontawesome/css/all.min.css",
                        "~/vendors/nice-select/nice-select.css",
                        "~/vendors/nouislider/nouislider.min.css",
                        "~/vendors/user/user.css"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include(
                    "~/vendors/jquery.form.js"));

            // Бандл для jQuery Validation Plugin
            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidate").Include(
                        "~/vendors/jquery.validate.min.js"));

            // Бандл для контактного скрипта
            bundles.Add(new ScriptBundle("~/bundles/contact").Include(
                        "~/vendors/contact.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                    "~/vendors/nouislider/nouislider.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                   "~/vendors/jquery/jquery-3.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/vendors/bootstrap/bootstrap.bundle.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/skrollr").Include(
                      "~/vendors/skrollr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/owlcarousel").Include(
                      "~/vendors/owl-carousel/owl.carousel.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/niceselect").Include(
                      "~/vendors/nice-select/jquery.nice-select.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ajaxchimp").Include(
                      "~/vendors/jquery.ajaxchimp.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/mailscrip").Include(
                      "~/vendors/mail-script.js"));

            // Ваш основной JS файл
            bundles.Add(new ScriptBundle("~/bundles/mainjs").Include(
                      "~/js/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/user").Include(
                     "~/vendors/user/select.min.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}