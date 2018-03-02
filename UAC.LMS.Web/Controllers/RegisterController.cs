using System.Web.Mvc;
using UAC.LMS.DAL;
using UAC.LMS.Models;

namespace UAC.LMS.Web.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        UnitOfWork unitofwork = new UnitOfWork();

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LMSLogin model)
        {
            if (ModelState.IsValid)
            {
                unitofwork.LMSLoginRepository.Register(model);
                unitofwork.Save();
                return RedirectToAction("Index", "Login");
            }
            return View(model);
        }
    }
}