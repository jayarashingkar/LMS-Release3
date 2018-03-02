using System.Web.Mvc;
using UAC.LMS.Models;
using UAC.LMS.Web.Filters;

namespace UAC.LMS.Web.Controllers
{
    [LMSAuthActionFilter]
    public class BaseController : Controller
    {
        public CurrentUser LoggedInUser
        {
            // comment here     
            // returns current User that is logged in that is stored in session
            get
            {
                CurrentUser currentUser = null;
                if (Session["CurrentUser"] != null)
                {
                    currentUser = (CurrentUser)Session["CurrentUser"];
                }
                return currentUser;
            }

        }

    }
}