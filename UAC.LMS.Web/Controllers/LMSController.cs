using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UAC.LMS.DAL;
using UAC.LMS.Models;
using UAC.LMS.Web.Models;

namespace UAC.LMS.Web.Controllers
{
    //[Authorize]
    public class LMSController : BaseController
    {
        // GET: LMS
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(DataGridoption option)
        {
            // comment here 

            LMSDBContext context = null;
            List<TestModel1> lstTestModel = null;
            int total = 0;
            IQueryable<TestModel1> query = null;
            try
            {
                context = new LMSDBContext();
                //query = context.TestModel1;

                if (!string.IsNullOrEmpty(option.searchBy) && !option.searchBy.Equals(""))
                {
                    query = from p in query
                            where (p.MyProperty1.ToLower().Contains(option.searchBy)
                            || p.MyProperty2.ToLower().Contains(option.searchBy)
                            || p.MyProperty3.ToLower().Contains(option.searchBy)
                            )
                            select p;
                }
                lstTestModel = query.OrderBy(x => x.TestModel1Id).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                //total = context.TestModel1.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstTestModel, total = total }, JsonRequestBehavior.AllowGet);
        }
    }
}




