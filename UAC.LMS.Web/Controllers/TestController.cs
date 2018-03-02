using System;
using System.Linq;
using System.Web.Mvc;
using UAC.LMS.DAL;

namespace UAC.LMS.Web.Controllers
{
    public class TestController : BaseController
    {        
        public ActionResult Index()
        {
            LMSDBContext context = null;
            try
            {
                context = new LMSDBContext();
                //var data = context.TestModel1.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        //[HttpPost]
        //public JsonResult GetData(DataTableParameters dataTableParameters)
        //{
        //    LMSContext context = null;
        //    List<TestModel1> lstTestModel = null;
        //    try
        //    {
        //        context = new LMSContext();
        //        lstTestModel = context.TestModel1.ToList();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return Json("", JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult DataHandler(DTParameters param)
        //{
        //    LMSContext context = null;
        //    List<TestModel1> dtsource = null;
        //    List<string> likestr = new List<string>();
        //    try
        //    {
        //        context = new LMSContext();
        //        //dtsource = context.TestModel1.ToList();
        //        string search = param.Search.Value;
        //        if (!string.IsNullOrEmpty(search))
        //        {
        //            likestr.Add(search);
        //        }
        //        List<String> columnSearch = new List<string>();

        //        foreach (var col in param.Columns)
        //        {
        //            columnSearch.Add(col.Search.Value);
        //        }
        //        List<string> columnFilters = columnSearch;
        //        var id = Convert.ToInt32((columnFilters[0]));
        //        //var likestr =  string.Format("%{0}%", search.ToLower());
        //        dtsource = context.TestModel1.Where(p => (search == null                    
        //            || p.MyProperty1 != null && likestr.Any(term => p.MyProperty1.Contains(term))
        //            || p.MyProperty2 != null && likestr.Any(term => p.MyProperty2.Contains(term))
        //            || p.MyProperty3 != null && likestr.Any(term => p.MyProperty3.Contains(term))
        //        //|| p.MyProperty2 != null && p.MyProperty2.ToLower().Contains(search.ToLower())
        //        //|| p.MyProperty3 != null && p.MyProperty3.ToLower().Contains(search.ToLower())
        //        )
        //        && (columnFilters[0] == null || (p.TestModel1Id > 0 && p.TestModel1Id == id))
        //        && (columnFilters[1] == null || (p.MyProperty1 != null && columnFilters[1].Any(term => p.MyProperty1.Contains(term))))
        //        && (columnFilters[2] == null || (p.MyProperty2 != null && columnFilters[2].Any(term => p.MyProperty2.Contains(term))))
        //        && (columnFilters[3] == null || (p.MyProperty3 != null && columnFilters[3].Any(term => p.MyProperty3.Contains(term))))


        //        ).ToList();

        //        int count = dtsource.Count;
        //        DTResult<TestModel1> result = new DTResult<TestModel1>
        //        {
        //            draw = param.Draw,
        //            data = dtsource,
        //            recordsFiltered = count,
        //            recordsTotal = count
        //        };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { error = ex.Message });
        //    }
        //}
    }
}