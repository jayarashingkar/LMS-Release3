using System.Web.Mvc;
using System.Web.Routing;
using UAC.LMS.Models;

namespace UAC.LMS.Web.Filters
{
    // comment here 

  
    public class LMSAuthActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CurrentUser currentUser = (CurrentUser)filterContext.HttpContext.Session.Contents["CurrentUser"];
            if (currentUser == null || currentUser.UserId <= 0)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Index",
                    Controller = "Login"
                }));
            }
            else
            {
                string actionName = filterContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                if (controllerName != "LogOut" && !(actionName == "SecuityConfig" && controllerName == "Admin"))
                {
                    if (!currentUser.IsSecurityApplied)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            action = "SecuityConfig",
                            Controller = "Admin"              

                        }));
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}