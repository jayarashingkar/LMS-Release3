using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UAC.LMS.Web.Controllers;
using UAC.LMS.Models;
using UAC.LMS.Common.Utilities;
using UAC.LMS.DAL;

namespace UAC.LMS.Web.Controllers
{
    public class LogOutController : BaseController
    {
        // GET: LogOut
        public ActionResult Index()
        {

            Session.Abandon();
            //Response.Cookies["LMSLogin"].Expires = DateTime.Now.AddDays(-1);
            //HttpCookie cookie = new HttpCookie("LMSLogin");
            //cookie.Values.Add("UserName", LoggedInUser.UserName);
            //cookie.Values.Add("Password", string.Empty);
            //cookie.Expires = DateTime.Now.AddDays(15);
            //Response.Cookies.Add(cookie);

            LMSAudit lmsAudit = new LMSAudit();
            UnitOfWork unitofwork = new UnitOfWork();

            lmsAudit.TransactionDate = DateTime.Now;
            //lmsAudit.UserName = currentUser.UserName;
            //lmsAudit.FullName = currentUser.FullName;
            lmsAudit.UserName = LoggedInUser.UserName;
            lmsAudit.FullName = LoggedInUser.FullName;

            // make an entry in Audit table for user logging out
            lmsAudit.Section = "Logout";
            lmsAudit.Action = "Logging Out";
            lmsAudit.Description = String.Format(" User Name : {0}, Name: {1} Logged Out", LoggedInUser.UserName, LoggedInUser.FullName);

            unitofwork.LMSAuditRepository.Insert(lmsAudit);
            unitofwork.Save();

            return RedirectToAction("Index", "Login");


        }
    }
}