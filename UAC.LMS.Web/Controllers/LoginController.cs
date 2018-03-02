using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UAC.LMS.Common.Constants;
using UAC.LMS.DAL;
using UAC.LMS.Models;
using UAC.LMS.Common.Utilities;

namespace UAC.LMS.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        UnitOfWork unitofwork = new UnitOfWork();

        public ActionResult Index()
        {
            // comment here 

            ///<summary>
            ///
            ///</summary>
            LMSLogin login = new LMSLogin();
            if (Request.Cookies["LMSLogin"] != null)
            {
                var loginCookie = Request.Cookies["LMSLogin"];
                if (loginCookie != null && loginCookie.Values.Count > 0)
                {
                    login.UserName = loginCookie.Values["UserName"];
                    login.Password = loginCookie.Values["Password"];
                }
            }                  
            return View(login);
        }

        [HttpPost]
        public ActionResult Index(LMSLogin model)
        {
            ///<summary>
            ///Perform Login checks
            ///</summary>
            LMSLogin dbUser = new LMSLogin();
            if (ModelState.IsValid)
            {
                // Checks the Current user login and password matches and check the Permission level

                bool userExists = unitofwork.LMSLoginRepository.CheckIfUserExists(model.UserName);
                if (userExists)
                {
                    dbUser = unitofwork.LMSLoginRepository.CheckPassword(model);
                    if (dbUser != null)
                    {
                        CurrentUser currentUser = new CurrentUser
                        {
                            UserId = dbUser.UserId,
                            UserName = dbUser.UserName,
                            FullName = dbUser.FirstName + " " + dbUser.LastName,
                            PermissionLevel = dbUser.PermissionLevel,
                            IsSecurityApplied = dbUser.IsSecurityApplied,
                        };
                        
                        this.HttpContext.Session["CurrentUser"] = currentUser;
                                  
                        // add a record of User logging In.    
                                
                        LMSAudit lmsAudit = new LMSAudit();
                        lmsAudit.TransactionDate = DateTime.Now;
                        lmsAudit.UserName = currentUser.UserName;
                        lmsAudit.FullName = currentUser.FullName;
                        lmsAudit.Section = "Login";
                        lmsAudit.Action = "Logging In";
                        lmsAudit.Description = String.Format(" User Name : {0}, Name: {1} Logged In. Permission = {2}", currentUser.UserName, currentUser.FullName, currentUser.PermissionLevel);
                        unitofwork.LMSAuditRepository.Insert(lmsAudit);
                        unitofwork.Save();

                        //saves the user login name in cookies - for Remember Me option - for 15 days
                        if (model.IsRememberMe)
                        {
                            HttpCookie cookie = new HttpCookie("LMSLogin");
                            cookie.Values.Add("UserName", currentUser.UserName);
                            //cookie.Values.Add("Password", model.Password);
                            cookie.Expires = DateTime.Now.AddDays(15);
                            Response.Cookies.Add(cookie);
                            
                        }
                        else
                        {
                            Response.Cookies["LMSLogin"].Expires = DateTime.Now.AddDays(-1);
                            //HttpCookie cookie = new HttpCookie("LMSLogin");
                            //cookie.Values.Add("UserName", currentUser.UserName);
                            //cookie.Expires = DateTime.Now.AddDays(15);
                            //Response.Cookies.Add(cookie);
                        }
                        return RedirectToAction("EmployeeList", "Employee");
                    }
                    else
                    {
                        dbUser = new LMSLogin
                        {
                            UserName = model.UserName,
                            Message = "Wrong Password."
                        };
                    }
                }
                else
                    dbUser.Message = "User does not exists.";
            }
            return View(dbUser);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult LoadSecurity(string UserName)
        {
            ///<summary>
            /// Loads the Login Security Question and Answer 
            /// Stores the Answer for the corresponding chosen Question 
            /// </summary>
            LMSDBContext context = new LMSDBContext();
            List<SelectListItem> ddSecurityQuestions = new List<SelectListItem>();
            LMSLogin login = context.LMSLogins.SingleOrDefault(x => x.UserName == UserName);
            int questionId = 0;
            if (login != null)
            {

                LMSUserSecurityAnswer answer = context.LMSUserSecurityAnswers.SingleOrDefault(x => x.LMSLoginId == login.UserId);
                if (answer != null)
                {
                    questionId = answer.LMSSecurityQuestionId;
                    if (questionId > 0)
                    {
                        IEnumerable<LMSSecurityQuestion> securityQuestions = unitofwork.LMSSecurityQuestionRepository.Get(x => x.LMSSecurityQuestionId == questionId);
                        securityQuestions.ToList().ForEach(x => ddSecurityQuestions.Add(new SelectListItem { Text = x.Question, Value = x.LMSSecurityQuestionId.ToString(), Selected = true }));
                    }
                }
            }
            ViewBag.questionId = questionId;
            ViewBag.ddSecurityQuestions = ddSecurityQuestions;
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(LMSUserSecurityAnswer model)
        {
            ///<summary>
            /// To recover the forgotten password
            /// Checks the user name and the Security Answer if it matches and then stores the new password entered by the user
            ///</summary>
            bool isSuccess = false;
            string message = "";
            LMSLogin login = null;
            LMSDBContext context = null;
            LMSUserSecurityAnswer securityanswer = null;
            try
            {
                context = new LMSDBContext();
                login = context.LMSLogins.SingleOrDefault(x => x.UserName == model.UserName);
                if (login != null)
                {
                    securityanswer = context.LMSUserSecurityAnswers.SingleOrDefault(x => x.LMSLoginId == login.UserId);
                    if (securityanswer != null)
                    {
                        if (securityanswer.LMSSecurityQuestionId == model.LMSSecurityQuestionId && string.Equals(securityanswer.SecurityAnswer, model.SecurityAnswer, StringComparison.OrdinalIgnoreCase))
                        {
                            int charaters = CommonConstants.PasswordLength;
                            string newPassword = charaters.RandomString();
                            string strCurrentDate = DateTime.Now.ToString();
                            byte[] strSaltTemp = Encryptor.EncryptText(strCurrentDate, login.UserName);
                            string se = Convert.ToBase64String(strSaltTemp);
                            byte[] strPasswordHash = Encryptor.GenerateHash(newPassword, se.ToString());
                            login.PasswordHash = strPasswordHash;
                            login.PasswordSalt = strSaltTemp;
                            login.LastModifiedBy = login.UserId;
                            login.LastModifiedOn = DateTime.Now;
                            login.IsSecurityApplied = false;
                            context.SaveChanges();
                            isSuccess = true;
                            message = newPassword;
                        }
                        else
                            message = "Incorrect answer.";
                    }
                    else
                        message = "Security answer does not exists.";
                }
                else
                    message = "UserName does not exists.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}