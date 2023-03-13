using CsrfPlayGround.Attributes;
using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CsrfPlayGround.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken] // This attribute will do the Anti-Forgery token validation for you.
        public ActionResult Transfer()
        {
            return Json(true); // This line is added for brevity. You would be doing good stuff here.
        }

        [HttpPost]
        public ActionResult Manage()
        {
            string cookieToken = "";
            string formToken = "";

            var tokenHeaders = Request.Headers.GetValues("__RequestVerificationToken");
            string[] tokens = tokenHeaders?.First()?.Split(':');
            if (tokens.Length == 2)
            {
                cookieToken = tokens[0].Trim();
                formToken = tokens[1].Trim();
            }

            try
            {
                AntiForgery.Validate(cookieToken, formToken);
            }
            catch (Exception ex)
            {
                // Alert folks that someone is trying to attack this method.
            }

            return Json(true); // This line is added for brevity. You would be doing good stuff here.
        }

        [HttpPost]
        [ValidateHeaderAntiForgeryToken] // This is where we put the Anti-Forgery attribute we just created
        public ActionResult Manage2()
        {
            return Json(true); // This line is added for brevity. You would be doing good stuff here.
        }
    }
}