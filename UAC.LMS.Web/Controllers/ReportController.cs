using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using UAC.LMS.DAL;
using UAC.LMS.Web.ViewModel;

namespace UAC.LMS.Web.Controllers
{
    public class ReportController : BaseController
    {
        // Training OverDues Report
        public ActionResult TrainingOverDues()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TrainingOverDues(string data)
        {
            ExportToExcel("TrainingOverDues");
            return View();
        }

        // TrainingnotTaken Report
        public ActionResult TrainingnotTaken()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TrainingnotTaken(string data)
        {
            ExportToExcel("TrainingNotTaken");
            return View();
        }

        public void TrainingnotTaken1()
        {
            ExportToExcel("TrainingNotTaken");

        }
        public void TrainingOverDues1()
        {
            ExportToExcel("TrainingOverDues");

        }


        public void ExportToExcel(string view)
        {
            try
            {
                var gv = new GridView();
                string fileName = view + "_" + DateTime.Now.ToString().Replace(" ", "").Replace("-", "").Replace(":", "");
                if (view == "TrainingOverDues")
                    gv.DataSource = this.GetTrainingOverDues();
                else
                    gv.DataSource = this.GetTrainingNotTaken();
                gv.DataBind();
                Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                Response.Buffer = true;
                Response.ContentType = "application/ms-excel";
                Response.AppendHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                // Response.Output.Write("This is a test");
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                //   throw ex;
            }
        }
        //public void ExportToExcel(string view)
        //{
        //    try
        //    {
        //        var gv = new GridView();
        //        string fileName = view + "_" + DateTime.Now.ToString().Replace(" ", "").Replace("-", "").Replace(":", "");
        //        if (view == "TrainingOverDues")
        //            gv.DataSource = this.GetTrainingOverDues();
        //        else
        //            gv.DataSource = this.GetTrainingNotTaken();
        //        gv.DataBind();
        //        Response.Clear();
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
        //        Response.ContentType = "application/ms-excel";
        //        Response.Charset = "";
        //        StringWriter objStringWriter = new StringWriter();
        //        HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
        //        gv.RenderControl(objHtmlTextWriter);
        //        Response.Output.Write(objStringWriter.ToString());
        //        var trying = "trying";
        //        if (trying = "trying")
        //        {
        //            Response.Flush();
        //            Response.End();
        //        }

        //        Response.Flush();
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //    //var gv = new GridView();
            //string fileName = view + "_" + DateTime.Now.ToString().Replace(" ", "").Replace("-", "").Replace(":", "");
            //if (view == "TrainingOverDues")
            //    gv.DataSource = this.GetTrainingOverDues();
            //else
            //    gv.DataSource = this.GetTrainingNotTaken();
            //gv.DataBind();
            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
            //Response.ContentType = "application/ms-excel";
            //Response.Charset = "";
            //StringWriter objStringWriter = new StringWriter();
            //HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            //gv.RenderControl(objHtmlTextWriter);
            //Response.Output.Write(objStringWriter.ToString());           
            //Response.Flush(); 
            //Response.End();
        

        private List<LMSOverDuesTrainings_VM_Excel> GetTrainingOverDues()
        {
            LMSDBContext context = null;
            List<LMSOverDuesTrainings_VM> overdues = null;
            List<LMSOverDuesTrainings_VM_Excel> excel = new List<LMSOverDuesTrainings_VM_Excel>();
            context = new LMSDBContext();
            //overdues = (from emp in context.LMSEmployees
            //            join empcourse in context.LMSEmployeeCourses.Where(x => x.DueDate <= DateTime.Now) on emp.LMSEmployeeId equals empcourse.LMSEmployeeId
            //            join course in context.LMSCourses on empcourse.LMSCourseId equals course.LMSCourseId
            overdues = (from emp in context.LMSEmployees.Where(j => j.StatusCode.ToLower() == "active")
                         join empcourse in context.LMSEmployeeCourses.Where(x => x.DueDate <= DateTime.Now && x.CompletedDate < x.DueDate) on emp.LMSEmployeeId equals empcourse.LMSEmployeeId
                         join course in context.LMSCourses on empcourse.LMSCourseId equals course.LMSCourseId
                         select new LMSOverDuesTrainings_VM
                        {
                            //LMSEmployeeId = emp.LMSEmployeeId,
                            EmployeeName = emp.FirstName + ", " + emp.LastName,
                            EmployeeNo = emp.EmployeeNo,
                            TrainingEvent = course.CourseName,
                            Frequency = course.Frequency,
                            CourseNo = course.CourseCode,
                            Department = emp.LMSDepartment.DepartmentName,
                            CompletedDate = empcourse.CompletedDate,
                            DueDate = empcourse.DueDate,
                        }).ToList();

            if (overdues != null)
            {
                overdues.ForEach(x => excel.Add(new LMSOverDuesTrainings_VM_Excel
                {
                    Employee = x.EmployeeName,
                    EmployeeNo = x.EmployeeNo,
                    TrainingEvent = x.TrainingEvent,
                    Frq = x.Frequency,
                    CourseNo = x.CourseNo,
                    Department = x.Department,
                    DateCompleted = x.CompletedDate,
                    DateDue = x.DueDate,
                    DaysRemain = x.DaysRemain
                }));
            }
            return excel;
        }

        private List<LMSOverDuesTrainings_VM_Excel> GetTrainingNotTaken()
        {
            LMSDBContext context = null;
            List<LMSOverDuesTrainings_VM> overdues = null;
            List<LMSOverDuesTrainings_VM_Excel> excel = new List<LMSOverDuesTrainings_VM_Excel>();
            context = new LMSDBContext();
            //overdues = (from emp in context.LMSEmployees
            //            join empcourse in context.LMSEmployeeCourses.Where(x => x.CompletedDate == null) on emp.LMSEmployeeId equals empcourse.LMSEmployeeId
            //            join course in context.LMSCourses on empcourse.LMSCourseId equals course.LMSCourseId
            overdues = (from emp in context.LMSEmployees.Where(j => j.StatusCode.ToLower() == "active")
                        join empcourse in context.LMSEmployeeCourses.Where(x => x.CompletedDate == null) on emp.LMSEmployeeId equals empcourse.LMSEmployeeId
                        join course in context.LMSCourses on empcourse.LMSCourseId equals course.LMSCourseId
                        select new LMSOverDuesTrainings_VM
                        {
                            //LMSEmployeeId = emp.LMSEmployeeId,
                            EmployeeName = emp.FirstName + ", " + emp.LastName,
                            EmployeeNo = emp.EmployeeNo,
                            TrainingEvent = course.CourseName,
                            Frequency = course.Frequency,
                            CourseNo = course.CourseCode,
                            Department = emp.LMSDepartment.DepartmentName,
                            CompletedDate = empcourse.CompletedDate,
                            DueDate = empcourse.DueDate,
                        }).ToList();
            if (overdues != null)
            {
                overdues.ForEach(x => excel.Add(new LMSOverDuesTrainings_VM_Excel
                {
                    Employee = x.EmployeeName,
                    EmployeeNo = x.EmployeeNo,
                    TrainingEvent = x.TrainingEvent,
                    Frq = x.Frequency,
                    CourseNo = x.CourseNo,
                    Department = x.Department,
                    DateCompleted = x.CompletedDate,
                    DateDue = x.DueDate,
                    DaysRemain = x.DaysRemain
                }));
            }
            return excel;
        }
    }
}