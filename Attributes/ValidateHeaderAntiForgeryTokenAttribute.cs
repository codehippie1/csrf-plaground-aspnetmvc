using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CsrfPlayGround.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateHeaderAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        // Bonus method
        private void TellEveryoneWeBlockedAttacker(HttpContextBase httpContext, Exception ex)
        {
            string controllerName = httpContext?.Request?.RequestContext?.RouteData?.Values["controller"]?.ToString();
            string actionName = httpContext?.Request?.RequestContext?.RouteData?.Values["action"]?.ToString();

            ex.Data.Add("ControllerName", controllerName);
            ex.Data.Add("ActionName", actionName);

            // Use the exception we created to notify the logging or error reporting system.
            // You may alternatively send an email message or slack message here.
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpContext = filterContext.HttpContext;
            var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];

            try
            {
                AntiForgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Headers["__RequestVerificationToken"]);
            }
            catch (Exception ex)
            {
                TellEveryoneWeBlockedAttacker(httpContext, ex);
            }
        }
    }
}