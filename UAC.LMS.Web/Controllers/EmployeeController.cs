using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using UAC.LMS.Common.Constants;
using UAC.LMS.DAL;
using UAC.LMS.Models;
using UAC.LMS.Web.ViewModel;
using System.Data.Entity.Validation;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Text;
using System.Web;


using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace UAC.LMS.Web.Controllers
{
    /// <summary>
    /// Employee Entities
    /// </summary>
    public class EmployeeController : BaseController
    {
        UnitOfWork unitofwork = new UnitOfWork();

        #region Employee

        public ActionResult EmployeeList()
        {
            SelectListItem defaultItem = null;
            try
            {
                defaultItem = new SelectListItem { Text = "Please Select", Value = "-1" };

                // sort and store list of Departments in ViewBag.ddDepartments to access from Front End 

                IEnumerable<LMSDepartment> departments = unitofwork.LMSDepartmentRepository.Get(x => x.StatusCode == StatusCodeConstants.Active).OrderBy(j => j.DepartmentName);
                List<SelectListItem> ddDepartments = new List<SelectListItem>();
                ddDepartments.Add(defaultItem);
                departments.ToList().ForEach(x => ddDepartments.Add(new SelectListItem { Text = x.DepartmentName, Value = x.LMSDepartmentId.ToString() }));
                ViewBag.ddDepartments = ddDepartments;

                // sort and store list of Job Titles in ViewBag.ddJobTitles to access from Front End 

                IEnumerable<LMSJobTitle> jobTitles = unitofwork.LMSJobTitleRepository.Get(x => x.StatusCode == StatusCodeConstants.Active).OrderBy(j => j.JobTitleName);
                List<SelectListItem> ddJobTitles = new List<SelectListItem>();
                ddJobTitles.Add(defaultItem);
                jobTitles.ToList().ForEach(x => ddJobTitles.Add(new SelectListItem { Text = x.JobTitleName, Value = x.LMSJobTitleId.ToString(), Selected = false }));
                ViewBag.ddJobTitles = ddJobTitles;

                //IEnumerable<LMSBusinessUnit> businessUnits = unitofwork.LMSBusinessUnitRepository.Get(x => x.StatusCode == StatusCodeConstants.Active);
                //List<SelectListItem> ddBusinessUnits = new List<SelectListItem>();
                //ddBusinessUnits.Add(defaultItem);
                //businessUnits.ToList().ForEach(x => ddBusinessUnits.Add(new SelectListItem { Text = x.BusinessUnitName, Value = x.LMSBusinessUnitId.ToString() }));
                //ViewBag.ddBusinessUnits = ddBusinessUnits;

                // store list of Status in ViewBag.ddStatus to access from Front End 

                List<SelectListItem> ddStatus = new List<SelectListItem>();
                //ddStatus.Add(defaultItem);
                //ddStatus.Add(new SelectListItem { Text = "Active", Value = StatusCodeConstants.Active });
                //ddStatus.Add(new SelectListItem { Text = "On Leave", Value = StatusCodeConstants.OnLeave });
                //// Removed Retired 11/21/2016
                ////ddStatus.Add(new SelectListItem { Text = "Retired", Value = StatusCodeConstants.Retired });
                //ddStatus.Add(new SelectListItem { Text = "Terminated", Value = StatusCodeConstants.Terminated });
                //ViewBag.ddStatus = ddStatus;

                ddStatus.Add(defaultItem);
                ddStatus.Add(new SelectListItem { Text = "Active", Value = "Active" });
                ddStatus.Add(new SelectListItem { Text = "On Leave", Value = "On Leave" });
                // Removed Retired 11/21/2016
                //ddStatus.Add(new SelectListItem { Text = "Retired", Value = StatusCodeConstants.Retired });
                ddStatus.Add(new SelectListItem { Text = "Terminated", Value = "Terminated" });
                ViewBag.ddStatus = ddStatus;

                // store list of Status in ViewBag.Shifts to access from Front End 

                List<SelectListItem> ddShifts = new List<SelectListItem>();
                ddShifts.Add(defaultItem);
                ddShifts.Add(new SelectListItem { Text = "1st", Value = ShiftConstants.First });
                ddShifts.Add(new SelectListItem { Text = "2nd", Value = ShiftConstants.Second });
                ddShifts.Add(new SelectListItem { Text = "3rd", Value = ShiftConstants.Third });
                //ddShifts.Add(new SelectListItem { Text = "4th", Value = ShiftConstants.Fourth });
                ViewBag.ddShifts = ddShifts;

                // sort and store list of Business Unit Titles in ViewBag.ddBusinessUnits to access from Front End 

                IEnumerable<LMSBusinessUnit> businessUnits = unitofwork.LMSBusinessUnitRepository.Get(x => x.StatusCode == StatusCodeConstants.Active).OrderBy(j => j.BusinessUnitName);
                List<SelectListItem> ddBusinessUnits = new List<SelectListItem>();
                ddBusinessUnits.Add(defaultItem);
                businessUnits.ToList().ForEach(x => ddBusinessUnits.Add(new SelectListItem { Text = x.BusinessUnitName, Value = x.LMSBusinessUnitId.ToString() }));
                ViewBag.ddBusinessUnits = ddBusinessUnits;

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SaveEmployee(LMSEmployee model)
        {
            bool isSuccess = false;
            string message = string.Empty;
            int LMSEmployeeId = 0;

            try
            {
                if (model != null)
                {
                    #region Edit Employee
                    if (model.LMSEmployeeId > 0)
                    {
                        LMSEmployeeId = model.LMSEmployeeId;
                        var empCount = unitofwork.LMSEmployeeRepository.Get(x => x.LMSEmployeeId != model.LMSEmployeeId && x.EmployeeNo == model.EmployeeNo).Count();
                        if (empCount == 0)
                        {
                            // Edit Employee Information 

                            var entity = unitofwork.LMSEmployeeRepository.GetByID(model.LMSEmployeeId);

                            LMSEmployee oldEntity = new LMSEmployee();
                            oldEntity.EmployeeNo = entity.EmployeeNo;
                            oldEntity.FirstName = entity.FirstName;
                            oldEntity.LastName = entity.LastName;
                            oldEntity.MiddleName = entity.MiddleName;
                            oldEntity.HireDate = entity.HireDate;
                            oldEntity.Shift = entity.Shift;

                            oldEntity.LMSBusinessUnitId = entity.LMSBusinessUnitId;
                            var bussniessEntity = unitofwork.LMSBusinessUnitRepository.GetByID(entity.LMSBusinessUnitId);
                            var businessModel = unitofwork.LMSBusinessUnitRepository.GetByID(model.LMSBusinessUnitId);

                            oldEntity.LMSDepartmentId = entity.LMSDepartmentId;
                            var departmentEntity = unitofwork.LMSDepartmentRepository.GetByID(entity.LMSDepartmentId);
                            var departmentModel = unitofwork.LMSDepartmentRepository.GetByID(model.LMSDepartmentId);

                            oldEntity.LMSJobTitleId = entity.LMSJobTitleId;
                            var jobTitleEntity = unitofwork.LMSJobTitleRepository.GetByID(entity.LMSJobTitleId);
                            var jobTitleModel = unitofwork.LMSJobTitleRepository.GetByID(model.LMSJobTitleId);

                            entity.FirstName = model.FirstName;
                            entity.LastName = model.LastName;
                            entity.MiddleName = model.MiddleName;
                            entity.EmployeeNo = model.EmployeeNo;
                            entity.HireDate = model.HireDate;
                            entity.Shift = model.Shift;
                            entity.LMSDepartmentId = model.LMSDepartmentId;
                            entity.LMSBusinessUnitId = model.LMSBusinessUnitId;
                            entity.LMSJobTitleId = model.LMSJobTitleId;
                            entity.StatusCode = model.StatusCode;
                            entity.LastModifiedBy = LoggedInUser.UserId;
                            entity.LastModifiedOn = DateTime.Now;

                            //changing here

                            unitofwork.LMSEmployeeRepository.Update(entity);

                            unitofwork.Save();
                            isSuccess = true;

                            // Add a record to LMSAudit, of editing Employee information 

                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            lmsAudit.Section = "Employee";
                            lmsAudit.Action = "Edit";
                            lmsAudit.Description = String.Format("Edited Employee :{0}, {1} ", oldEntity.EmployeeNo, oldEntity.FullName);

                            if (oldEntity.FirstName != model.FirstName)
                                lmsAudit.Description += String.Format(" * Edited Employee First Name: {0} to {1}", oldEntity.FirstName, model.FirstName);
                            if (oldEntity.LastName != model.LastName)
                                lmsAudit.Description += String.Format(" * Edited Employee Last Name: {0} to {1}", oldEntity.LastName, model.LastName);
                            if (oldEntity.MiddleName != model.MiddleName)
                                lmsAudit.Description += String.Format(" * Edited Employee Middle Name: {0} to {1}", oldEntity.MiddleName, model.MiddleName);
                            if (oldEntity.EmployeeNo != model.EmployeeNo)
                                lmsAudit.Description += String.Format(" * Edited Employee Number: {0} to {1}", oldEntity.EmployeeNo, model.EmployeeNo);
                            if (oldEntity.HireDate != model.HireDate)
                                lmsAudit.Description += String.Format(" * Edited Employee Hire Date: {0} to {1}", oldEntity.HireDate, model.HireDate);
                            if (oldEntity.Shift != model.Shift)
                                lmsAudit.Description += String.Format(" * Edited Employee Shift: {0} to {1}", oldEntity.Shift, model.Shift);
                            if (oldEntity.LMSDepartmentId != model.LMSDepartmentId)
                                lmsAudit.Description += String.Format(" * Edited Employee Department: {0} to {1}", departmentEntity.DepartmentName, departmentModel.DepartmentName);

                            // Changed  Business Unit to  Location 11/21/2016
                            if (oldEntity.LMSBusinessUnitId != model.LMSBusinessUnitId)
                                // lmsAudit.Description += String.Format(" * Edited Employee Business Unit: {0} to {1}", bussniessEntity.BusinessUnitName, businessModel.BusinessUnitName);
                                lmsAudit.Description += String.Format(" * Edited Employee Location: {0} to {1}", bussniessEntity.BusinessUnitName, businessModel.BusinessUnitName);

                            if (oldEntity.LMSJobTitleId != model.LMSJobTitleId)
                                lmsAudit.Description += String.Format(" * Edited Employee Job Title: {0} to {1}", jobTitleEntity.JobTitleName, jobTitleModel.JobTitleName);

                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();
                        }
                        else
                            message = "Employee with same employee number already exists.";
                    }
                    #endregion
                    else
                    {
                        //Message_VM result = new Message_VM();
                        //result = saveEmployeeInfo(model);
                        //message = result.Message;
                        //isSuccess = result.isSuccess; 
                        LMSDBContext context = new LMSDBContext();
                        var empCount = context.LMSEmployees.Count(x => x.EmployeeNo == model.EmployeeNo);
                        if (empCount == 0)
                        {
                            // Add new Employee record in LMSEmployee
                            model.CreatedBy = LoggedInUser.UserId;
                            model.CreatedOn = DateTime.Now;
                            unitofwork.LMSEmployeeRepository.Insert(model);
                            unitofwork.Save();
                            isSuccess = true;

                            // Add new record in LMSAudit of Adding new Employee record in LMSEmployee                            
                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            lmsAudit.Section = "Employee";
                            lmsAudit.Action = "Add";
                            lmsAudit.Description = String.Format("Added new Employee: {0} (Employee No:{1}) ", model.FullName, model.EmployeeNo);
                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();
                            LMSEmployeeId = model.LMSEmployeeId;
                        }
                        else
                            message = "Employee with same employee number already exists.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message, LMSEmployeeId = LMSEmployeeId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditEmployee(int id)
        {
            // returns the Employee record that is edited.
            LMSEmployee entity = null;
            try
            {
                entity = unitofwork.LMSEmployeeRepository.GetByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { entity = entity }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteEmployee(int id)
        {
            // Deleletes an employee 

            LMSEmployee entity = null;
            bool isSuccess = false;
            try
            {
                entity = unitofwork.LMSEmployeeRepository.GetByID(id);
                entity.LastModifiedOn = DateTime.Now;
                entity.StatusCode = StatusCodeConstants.InActive;
                //changing here 
                //unitofwork.LMSEmployeeRepository.Update(entity);
                unitofwork.LMSEmployeeRepository.Delete(entity);
                unitofwork.Save();
                isSuccess = true;

                // add a record in LMSAudit table of deleting Employee

                LMSAudit lmsAudit = new LMSAudit();
                lmsAudit.TransactionDate = DateTime.Now;
                lmsAudit.UserName = LoggedInUser.UserName;
                lmsAudit.FullName = LoggedInUser.FullName;
                lmsAudit.Section = "Employee";
                lmsAudit.Action = "Delete";
                lmsAudit.Description = String.Format("Deleted Employee: {0} (Employee No: {1}) ", entity.FullName, entity.EmployeeNo);
                unitofwork.LMSAuditRepository.Insert(lmsAudit);
                unitofwork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess }, JsonRequestBehavior.AllowGet);
        }

        //      public ActionResult SaveEmployee(LMSEmployee model)

        //     public ActionResult ImportEmployeeList(LMSEmployee model)

       private Message_VM saveEmployeeInfo(LMSEmployee model)
        {
            Message_VM result = new Message_VM();
            result.isSuccess = true;
            result.Message = "";

            LMSDBContext context = new LMSDBContext();
            var empCount = context.LMSEmployees.Count(x => x.EmployeeNo == model.EmployeeNo);
            if (empCount == 0)
            {
                // Add new Employee record in LMSEmployee
                model.CreatedBy = LoggedInUser.UserId;
                model.CreatedOn = DateTime.Now;
                unitofwork.LMSEmployeeRepository.Insert(model);
                unitofwork.Save();
                result.isSuccess = true;
                result.Message = "File Imported";

                // Add new record in LMSAudit of Adding new Employee record in LMSEmployee                            
                LMSAudit lmsAudit = new LMSAudit();
                lmsAudit.TransactionDate = DateTime.Now;
                lmsAudit.UserName = LoggedInUser.UserName;
                lmsAudit.FullName = LoggedInUser.FullName;
                lmsAudit.Section = "Employee";
                lmsAudit.Action = "Add";
                lmsAudit.Description = String.Format("Added new Employee: {0} (Employee No:{1}) ", model.FullName, model.EmployeeNo);
                unitofwork.LMSAuditRepository.Insert(lmsAudit);
                unitofwork.Save();
             //   LMSEmployeeId = model.LMSEmployeeId;
            }
            else
            {
                result.Message = model.EmployeeNo;
                result.isSuccess = false;
            }
                
            return result;
        }
        #region import employee  
        public ActionResult ImportEmployeeList(string Message ="none")
        {
            if (Message.Trim()!= "none")
                 ViewBag.Message = Message;
            else
                ViewBag.Message =  "";

            return View();

        }


        //[HttpPost]
        //public ActionResult UploadExcel(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        //var fileName = Path.GetFileName(file.FileName);
        //        //BinaryReader b = new BinaryReader(file.InputStream);
        //        //byte[] binData = b.ReadBytes(file.ContentLength);
        //        //string result = System.Text.Encoding.UTF8.GetString(binData);
        //        string path = Path.GetTempPath() + file.FileName;
        //        path = "C:\\Users\\jaya.rashingkar\\AppData\\Local\\Temp\\Import Employees Test3.xlsx";

        //        try
        //        {

        //            file.SaveAs(path);
        //            new EmployeeParser().ParseEmployeeExcel(path);

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            System.IO.File.Delete(path);
        //        }
        //        ViewBag.Message = "Excel File parsed successfully";
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Excel File not selected";
        //    }

        //    return View("Index");
        //}
        [HttpPost]

        // public ActionResult SaveImportEmployees(HttpPostedFileBase file)
        public ActionResult ImportEmployees(HttpPostedFileBase file)
        {
            Message_VM returnResult = new Message_VM();
            returnResult.isSuccess = true;
            returnResult.Message = "";
            var fileName = Path.GetFileName(file.FileName);
            BinaryReader b = new BinaryReader(file.InputStream);
            byte[] binData = b.ReadBytes(file.ContentLength);
            string result = System.Text.Encoding.UTF8.GetString(binData);
            //string filePath = Path.GetTempPath() + file.FileName;
            string filePath = Path.GetTempPath() + fileName;
            file.SaveAs(filePath);
          
            if (filePath != null)
            {
                try
                {
                    returnResult = ParseEmployeeExcel(filePath);
                    if (returnResult.isSuccess)
                        returnResult.Message = "File successfully imported";
                    else
                        returnResult.Message = "Import Error for : " + returnResult.Message;

                }
                catch (Exception ex)
                    {
                    //throw ex;
                    returnResult.isSuccess = false;
                    returnResult.Message = ex.ToString();
                    }
                finally
                {
                    // System.IO.File.Delete(path);
                    System.IO.File.Delete(filePath);
                }      
            }
            else
            {
                returnResult.Message = "Excel File not selected";
            }

            //   return Json(new { isSuccess = isSuccess, message = Message, LMSEmployeeId = LMSEmployeeId }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("ImportEmployeeList", new { Message = returnResult.Message });

    }

        private Message_VM ParseEmployeeExcel(string path)
        {
            //List<ImportEmployee_VM> result = null;
            Message_VM result = new Message_VM();
            result.isSuccess = true;
            result.Message = "";
            try
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(path);
                String[] excelSheets = new String[excelBook.Worksheets.Count];
                int i = 0;
                foreach (Microsoft.Office.Interop.Excel.Worksheet wSheet in excelBook.Worksheets)
                {
                    excelSheets[i] = wSheet.Name;
                    i++;
                }
                excelBook.Close();

                result = ParseExcel(path, excelSheets[0]);

            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message.ToString();
               // throw ex;                
            }
            return result;
        }

       //   private List<ImportEmployee_VM> ParseExcel(string path, string sheetName)
        private Message_VM ParseExcel(string path, string sheetName)
        {
            //bool isSuccess = true;
            Message_VM result = new Message_VM();
            result.isSuccess = true;
            result.Message = "";
            List<ImportEmployee_VM> employeeInfo = new List<ImportEmployee_VM>();
            try
            {              
                OleDbConnection oledbConn = OpenConnection(path);
                if (oledbConn.State == ConnectionState.Open)
                {
                    try
                    {
                         employeeInfo = ExtractEmployeeInfo(oledbConn, sheetName);
                        result = EmployeeInfo(employeeInfo);
                        // isSuccess = ExtractEmployeeInfo(oledbConn, sheetName);
                        oledbConn.Close();
                    }
                    finally
                    {
                        oledbConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message.ToString();
                //throw ex;
            }
            // return employeeInfo;
           return result;
        }

         private List<ImportEmployee_VM> ExtractEmployeeInfo(OleDbConnection oledbConn, string sheetName)
       // private bool ExtractEmployeeInfo(OleDbConnection oledbConn, string sheetName)
        {
       //     bool isSuccess = true;

            OleDbCommand cmd = new OleDbCommand(); ;
            OleDbDataAdapter oleda = new OleDbDataAdapter();
             DataSet employeeInfo = new DataSet();
            //   DataTable employeeInfo = new DataTable();
            List<ImportEmployee_VM> dsEmployeeInfoList = new List<ImportEmployee_VM>();
            try
            {
                cmd.Connection = oledbConn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM [" + sheetName + "$]"; //Excel Sheet Name ( Employee )
            oleda = new OleDbDataAdapter(cmd);
            //  oleda.Fill(employeeInfo, "Employee");
            oleda.Fill(employeeInfo);
                //  oleda.Fill(employeeInfo,sheetName);

                #region Fill Employee
               
                foreach (DataTable table in employeeInfo.Tables)
                {
                    foreach (DataRow listEmployeeData in table.Rows)

                    {
                        // LMSEmployee model = new LMSEmployee();
                        ImportEmployee_VM model = new ImportEmployee_VM();
                        model.LMSEmployeeId = 0;
                        model.EmployeeNo = listEmployeeData[0].ToString();
                        model.FirstName = listEmployeeData[1].ToString();
                        model.LastName = listEmployeeData[2].ToString();
                        model.MiddleName = listEmployeeData[3].ToString();
                        model.Email = listEmployeeData[4].ToString();
                        model.HireDate = Convert.ToDateTime(listEmployeeData[5]);                        
                      //  model.IsInitialOrientationApplied = false;
                        model.LMSDepartmentId = Convert.ToInt32(listEmployeeData[6]);                       
                        model.DepartmentCode = Convert.ToString(listEmployeeData[7]);
                        model.LMSJobTitleId = Convert.ToInt32(listEmployeeData[8]);
                        model.JobTitleName = Convert.ToString(listEmployeeData[9]);
                        model.LMSBusinessUnitId = Convert.ToInt32(listEmployeeData[10]);
                        model.DepartmentName = Convert.ToString(listEmployeeData[11]);                       
                        model.Status = listEmployeeData[12].ToString();
                        model.Shift = listEmployeeData[13].ToString();

                        dsEmployeeInfoList.Add(model);
                     
                    }
                }
            }


            #endregion

            #region Fill in Table
            //var dsEmployeeInfoList = employeeInfo.Tables[0].AsEnumerable().Where(s => s["EmployeeNo"] != DBNull.Value
            //                                                                && s["FirstName"] != DBNull.Value
            //                                                                && s["LastName"] != DBNull.Value
            //                                                                && s["MiddleName"] != DBNull.Value
            //                                                                && s["Email"] != DBNull.Value
            //                                                                && s["HireDate"] != DBNull.Value
            //                                                                && s["LMSDepartmentId"] != DBNull.Value
            //                                                                && s["DepartmentCode"] != DBNull.Value
            //                                                                && s["LMSJobTitleId"] != DBNull.Value
            //                                                                && s["JobTitleName"] != DBNull.Value
            //                                                                && s["LMSBusinessUnitId"] != DBNull.Value
            //                                                                && s["DepartmentName"] != DBNull.Value
            //                                                                && s["Status"] != DBNull.Value
            //                                                                && s["Shift"] != DBNull.Value).Select(s => new ImportEmployee_VM
            //                                                                {
            //                                                                    EmployeeNo = Convert.ToString(s["EmployeeNo"] != DBNull.Value ? s["EmployeeNo"] : ""),
            //                                                                    FirstName = Convert.ToString(s["FirstName"] != DBNull.Value ? s["FirstName"] : ""),
            //                                                                    MiddleName = Convert.ToString(s["MiddleName"] != DBNull.Value ? s["MiddleName"] : ""),
            //                                                                    Email = Convert.ToString(s["Email"] != DBNull.Value ? s["Email"] : ""),
            //                                                                    HireDate = Convert.ToDateTime(s["HireDate"] != DBNull.Value ? s["HireDate"] : ""),
            //                                                                    LMSDepartmentId = Convert.ToInt32(s["LMSDepartmentId"] != DBNull.Value ? s["LMSDepartmentId"] : ""),
            //                                                                    DepartmentCode = Convert.ToString(s["DepartmentCode"] != DBNull.Value ? s["DepartmentCode"] : ""),
            //                                                                    LMSJobTitleId = Convert.ToInt32(s["LMSJobTitleId"] != DBNull.Value ? s["LMSJobTitleId"] : ""),
            //                                                                    JobTitleName = Convert.ToString(s["JobTitleName"] != DBNull.Value ? s["JobTitleName"] : ""),
            //                                                                    LMSBusinessUnitId = Convert.ToInt32(s["LMSBusinessUnitId"] != DBNull.Value ? s["LMSBusinessUnitId"] : ""),
            //                                                                    DepartmentName = Convert.ToString(s["DepartmentName"] != DBNull.Value ? s["DepartmentName"] : ""),
            //                                                                    Status = Convert.ToString(s["Status"] != DBNull.Value ? s["Status"] : ""),
            //                                                                    Shift = Convert.ToString(s["Shift"] != DBNull.Value ? s["Shift"] : "")
            //                                                                }).ToList();
            #endregion

            catch (Exception ex)
            {
                // isSuccess = false;
                throw ex;
            }
            return dsEmployeeInfoList;
            //return isSuccess;
        }

        private OleDbConnection OpenConnection(string path)
        {
            // path = "C:\\New Development\\LMS RELEASE\\LMS RELEASE 3\\Import Employees\\ImportEmployeeTest1.xlsx";
            //path = "C:\\Users\\jaya.rashingkar\\AppData\\Local\\Temp\\Import Employees Test3.xlsx";
            OleDbConnection oledbConn = null;
            try
            {
                if (Path.GetExtension(path) == ".xls")
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + path + "; Extended Properties= \"Excel 8.0;HDR=Yes;IMEX=2\"");
                else if (Path.GetExtension(path) == ".xlsx")
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + path + "; Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");

                oledbConn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oledbConn;
        }

        private Message_VM EmployeeInfo(List<ImportEmployee_VM>  employeeInfo)
        {
            Message_VM result = new Message_VM();
          
           result.Message = "";
            string errorMessage = "Employees not saved: ";
            try
            {                
                foreach (var importedEmployee in employeeInfo)
                {
                    LMSEmployee employee = new LMSEmployee();

                    var jobTitlecount = unitofwork.LMSJobTitleRepository.Get(x => x.LMSJobTitleId == importedEmployee.LMSJobTitleId && x.JobTitleName.Trim() == importedEmployee.JobTitleName.Trim()).Count();
                    //     var departmentCount = unitofwork.LMSDepartmentRepository.Get(x => x.LMSDepartmentId != importedEmployee.LMSDepartmentId && x.DepartmentName == importedEmployee.DepartmentName).Count();
                    var departmentCount = unitofwork.LMSDepartmentRepository.Get(x => x.LMSDepartmentId == importedEmployee.LMSDepartmentId && x.DepartmentName.ToString().Contains(importedEmployee.DepartmentName)).Count();
                    var bussnessUnitCount = unitofwork.LMSBusinessUnitRepository.Get(x => x.LMSBusinessUnitId == importedEmployee.LMSBusinessUnitId).Count();
                                        
                    if (departmentCount > 0)
                    {
                        employee.LMSDepartmentId = importedEmployee.LMSDepartmentId;                                           
                    }
                    else
                    {
                        errorMessage += employee.EmployeeNo;
                      //  result.Message += employee.EmployeeNo;
                        result.isSuccess = false;
                        continue;
                    }

                    if (jobTitlecount > 0)
                    {
                        employee.LMSJobTitleId = importedEmployee.LMSDepartmentId;                        
                    }
                    else
                    {
                        errorMessage += employee.EmployeeNo;
                      //  result.Message += employee.EmployeeNo;
                        result.isSuccess = false;
                        continue;
                    }

                    if (bussnessUnitCount > 0)
                    {
                        employee.LMSBusinessUnitId = importedEmployee.LMSBusinessUnitId;
                    }
                    else
                    {
                        errorMessage += employee.EmployeeNo;
                       // result.Message += employee.EmployeeNo;
                        result.isSuccess = false;
                        continue;
                    }

                    employee.EmployeeNo = importedEmployee.EmployeeNo;
                    employee.FirstName = importedEmployee.FirstName;
                    employee.LastName = importedEmployee.LastName;
                    employee.MiddleName = importedEmployee.MiddleName;
                    employee.HireDate = importedEmployee.HireDate;
                    
                    employee.Email = importedEmployee.Email;
                    employee.Shift = importedEmployee.Shift;

                    employee.IsInitialOrientationApplied = false;
                    employee.StatusCode = "Active";

                    result = saveEmployeeInfo(employee);
                    if (errorMessage.Trim() == "Employees not saved: ")                        
                        errorMessage += result.Message;
                    else
                        errorMessage += ", "+result.Message;
                }
                result.Message = errorMessage;
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.ToString();
            }
            
            return result;
        }

        #endregion
        #region EmployeeCourse

        public ActionResult EmployeeCourseList(int id)
        {
            // List of Employee Courses 

            SelectListItem defaultItem = null;
            LMSDBContext context = null;
            LMSEmployeeCourse_VM employee = new LMSEmployeeCourse_VM();
            try
            {
                context = new LMSDBContext();
                defaultItem = new SelectListItem { Text = "Please Select", Value = "-1" };

                // store list of Employees in ViewBag.ddEmployees to access from front end

                IEnumerable<LMSEmployee> employees = unitofwork.LMSEmployeeRepository.Get(x => x.StatusCode == StatusCodeConstants.Active);
                List<SelectListItem> ddEmployees = new List<SelectListItem>();
                ddEmployees.Add(defaultItem);
                employees.ToList().ForEach(x => ddEmployees.Add(new SelectListItem { Text = x.FullName, Value = x.LMSEmployeeId.ToString() }));
                ViewBag.ddEmployees = ddEmployees;


                // for initial orienation courses only
                //IEnumerable<LMSCourse> jobCourses = unitofwork.LMSCourseRepository.Get(x => x.StatusCode == StatusCodeConstants.Active && x.IsInitialOrientation == true);

                // sort and store list of Courses in ViewBag.ddCourses to access from front end


                

                // sort and store list of Job Titles in ViewBag.ddJobTitles to access from front end

                IEnumerable<LMSJobTitle> jobTitles = unitofwork.LMSJobTitleRepository.Get(x => x.StatusCode == StatusCodeConstants.Active).OrderBy(j => j.JobTitleName);
                List<SelectListItem> ddJobTitles = new List<SelectListItem>();
                ddJobTitles.Add(defaultItem);
                jobTitles.ToList().ForEach(x => ddJobTitles.Add(new SelectListItem { Text = x.JobTitleName, Value = x.LMSJobTitleId.ToString(), Selected = false }));
                ViewBag.ddJobTitles = ddJobTitles;

                LMSEmployee dbentity = context.LMSEmployees.Include("LMSDepartment").Include("LMSJobTitle").Include("StatusCodeDetail")
                    .Include("LMSBusinessUnit").SingleOrDefault(x => x.LMSEmployeeId == id);
                if (dbentity != null)
                {
                    employee.LMSEmployeeId = dbentity.LMSEmployeeId;
                    employee.BusinessUnitName = dbentity.LMSBusinessUnit.BusinessUnitName;
                    employee.JobTitleId = dbentity.LMSJobTitleId;
                    employee.JobTitleName = dbentity.LMSJobTitle.JobTitleName;
                    employee.Shift = dbentity.Shift;
                    employee.DepartmentName = dbentity.LMSDepartment.DepartmentName;
                    employee.FirstName = dbentity.FirstName;
                    employee.LastName = dbentity.LastName;
                    employee.EmployeeNo = dbentity.EmployeeNo;
                    employee.HireDate = dbentity.HireDate;
                    employee.Status = dbentity.StatusCode;

                    string businessCourseCode = "";
                    var lmsBU = dbentity.LMSBusinessUnit;
                    if (lmsBU != null && !string.IsNullOrEmpty(lmsBU.BusinessUnitName))
                        businessCourseCode = lmsBU.BusinessUnitName[0].ToString();

                    IEnumerable<LMSCourse> jobCourses = unitofwork.LMSCourseRepository.Get(x => x.CourseCode.StartsWith(businessCourseCode) && x.StatusCode == StatusCodeConstants.Active).OrderBy(j => j.CourseName);
                    List<SelectListItem> ddCourses = new List<SelectListItem>();
                    ddCourses.Add(defaultItem);
                    jobCourses.ToList().ForEach(x => ddCourses.Add(new SelectListItem { Text = x.CourseName, Value = x.LMSCourseId.ToString() }));
                    ViewBag.ddCourses = ddCourses;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(employee);
        }


        public ActionResult CourseHistory(int id)
        {
            SelectListItem defaultItem = null;
            LMSEmployeeCourse_VM employee = new LMSEmployeeCourse_VM();
            try
            {
                // sort by Course Names and store Course names and codes in  ViewBag.ddCoursenames & ViewBag.ddCoursecodes to access from Front end

                defaultItem = new SelectListItem { Text = "Please Select", Value = "-1" };
                IEnumerable<LMSCourse> jobCourses = unitofwork.LMSCourseRepository.Get(x => x.StatusCode == StatusCodeConstants.Active).OrderBy(j => j.CourseName);
                List<SelectListItem> ddCoursenames = new List<SelectListItem>();
                List<SelectListItem> ddCoursecodes = new List<SelectListItem>();
                ddCoursenames.Add(defaultItem);
                ddCoursecodes.Add(defaultItem);
                jobCourses.ToList().ForEach(x => ddCoursenames.Add(new SelectListItem { Text = x.CourseName, Value = x.LMSCourseId.ToString() }));
                jobCourses.ToList().ForEach(x => ddCoursecodes.Add(new SelectListItem { Text = x.CourseCode, Value = x.LMSCourseId.ToString() }));
                ViewBag.ddCoursenames = ddCoursenames;
                ViewBag.ddCoursecodes = ddCoursecodes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(id);
        }

        [HttpPost]
        public ActionResult SaveEmployeeCourse(LMSEmployeeCourse model)
        {
            bool isSuccess = false;
            string message = string.Empty;
            try
            {
                if (model != null)
                {
                    if (model.LMSEmployeeCourseId > 0)
                    {
                        // Edit Employee Course information

                        bool isTrackCourse = false;
                        var entity = unitofwork.LMSEmployeeCourseRepository.GetByID(model.LMSEmployeeCourseId);
                        if (entity.CompletedDate != model.CompletedDate)
                            isTrackCourse = true;
                        LMSEmployeeCourse oldEntity = new LMSEmployeeCourse();
                        oldEntity.Evaluation = entity.Evaluation;
                        oldEntity.Remarks = entity.Remarks;
                        oldEntity.InstructorName = entity.InstructorName;
                        // Removed Previous Date 11/21/2016
                        // oldEntity.PreviousDate = entity.PreviousDate;
                        oldEntity.PreviousDate = entity.CompletedDate;
                        oldEntity.CompletedDate = entity.CompletedDate;
                        oldEntity.DueDate = entity.DueDate;

                        var employeeEntity = unitofwork.LMSEmployeeRepository.GetByID(entity.LMSEmployeeId);
                        var courseEntity = unitofwork.LMSCourseRepository.GetByID(entity.LMSCourseId);
                        var employeeModel = unitofwork.LMSEmployeeRepository.GetByID(model.LMSEmployeeId);
                        var courseModel = unitofwork.LMSCourseRepository.GetByID(model.LMSCourseId);

                        entity.LMSEmployeeId = model.LMSEmployeeId;
                        entity.LMSCourseId = model.LMSCourseId;
                        entity.Evaluation = model.Evaluation;
                        entity.Remarks = model.Remarks;
                        entity.InstructorName = model.InstructorName;
                        // Removed Previous Date 11/21/2016                     
                        // entity.PreviousDate = model.PreviousDate;
                        entity.PreviousDate = model.CompletedDate;
                        entity.CompletedDate = model.CompletedDate;
                        entity.DueDate = model.DueDate;
                        //entity.Frequency = model.Frequency;
                        //DateTime completed = (model.CompletedDate.HasValue) ? model.CompletedDate.Value : DateTime.Now;
                        //entity.DueDate = (model.Frequency > 0) ? completed.AddMonths(model.Frequency) : (DateTime?)null;
                        //entity.DueDate = model.DueDate;
                        entity.LastModifiedBy = LoggedInUser.UserId;
                        entity.LastModifiedOn = DateTime.Now;
                        unitofwork.LMSEmployeeCourseRepository.Update(entity);

                        // add in course history 

                        if (isTrackCourse)
                        {
                            LMSCourseHistory courseHistory = new LMSCourseHistory
                            {
                                LMSEmployeeId = entity.LMSEmployeeId,
                                LMSCourseId = entity.LMSCourseId,
                                CompletedDate = model.CompletedDate,
                                StatusCode = StatusCodeConstants.Active,
                                CreatedOn = DateTime.Now,
                                CreatedBy = LoggedInUser.UserId
                            };
                            unitofwork.LMSCourseHistoryRepository.Insert(courseHistory);
                        }
                        unitofwork.Save();
                        isSuccess = true;

                        // add record in LMSAudit for Editing the Employee Course 

                        LMSAudit lmsAudit = new LMSAudit();
                        lmsAudit.TransactionDate = DateTime.Now;
                        lmsAudit.UserName = LoggedInUser.UserName;
                        lmsAudit.FullName = LoggedInUser.FullName;
                        lmsAudit.Section = "Employee-Course";
                        lmsAudit.Action = "Edit";
                        lmsAudit.Description = String.Format("Edited Course: {0} taken by {1} (Employee Number: {2})", courseEntity.CourseName, employeeEntity.FullName, employeeEntity.EmployeeNo);

                        if (oldEntity.Evaluation != model.Evaluation)
                            lmsAudit.Description += String.Format(" * Edited Evaluation: {0} to {1}", oldEntity.Evaluation, model.Evaluation);
                        if (oldEntity.Remarks != model.Remarks)
                            lmsAudit.Description += String.Format(" * Edited Remarks: {0} to {1}", oldEntity.Remarks, model.Remarks);
                        if (oldEntity.InstructorName != model.InstructorName)
                            lmsAudit.Description += String.Format(" * Edited Instructor Name: {0} to {1}", oldEntity.InstructorName, model.InstructorName);
                        // Removed Previous Date 11/21/2016
                        //if (oldEntity.PreviousDate != model.PreviousDate)
                        //    lmsAudit.Description += String.Format(" * Edited Previous Date: {0} to {1}", oldEntity.PreviousDate, model.PreviousDate);
                        if (oldEntity.CompletedDate != model.CompletedDate)
                            lmsAudit.Description += String.Format(" * Edited Completed Date: {0} to {1}", oldEntity.CompletedDate, model.CompletedDate);
                        if (oldEntity.DueDate != model.DueDate)
                            lmsAudit.Description += String.Format(" * Edited Due Date: {0} to {1}", oldEntity.DueDate, model.DueDate);

                        unitofwork.LMSAuditRepository.Insert(lmsAudit);
                        unitofwork.Save();
                    }
                    else
                    {
                        // Add new course to Employee

                        LMSDBContext context = new LMSDBContext();
                        var entityCount = context.LMSEmployeeCourses.Count(x => x.LMSEmployeeId == model.LMSEmployeeId
                        && x.LMSCourseId == model.LMSCourseId);
                        if (entityCount == 0)
                        {
                            //DateTime completed = (model.CompletedDate.HasValue) ? model.CompletedDate.Value : DateTime.Now;
                            //model.DueDate = (model.Frequency > 0) ? completed.AddMonths(model.Frequency) : (DateTime?)null;
                            model.CreatedBy = LoggedInUser.UserId;
                            model.CreatedOn = DateTime.Now;
                            model.StatusCode = StatusCodeConstants.Active;
                            unitofwork.LMSEmployeeCourseRepository.Insert(model);

                            // Add course in Course History for the employee 

                            LMSCourseHistory courseHistory = new LMSCourseHistory
                            {
                                LMSEmployeeId = model.LMSEmployeeId,
                                LMSCourseId = model.LMSCourseId,
                                CompletedDate = model.CompletedDate,
                                StatusCode = StatusCodeConstants.Active,
                                CreatedOn = DateTime.Now,
                                CreatedBy = LoggedInUser.UserId
                            };
                            unitofwork.LMSCourseHistoryRepository.Insert(courseHistory);
                            unitofwork.Save();
                            isSuccess = true;

                            // add record in LMSAudit for Adding new course in Employee Course

                            var employeeModel = unitofwork.LMSEmployeeRepository.GetByID(model.LMSEmployeeId);
                            var courseModel = unitofwork.LMSCourseRepository.GetByID(model.LMSCourseId);

                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            lmsAudit.Section = "Employee-Course";
                            lmsAudit.Action = "Add";
                            lmsAudit.Description = String.Format("Added Course: {0} to {1} (Employee Number: {2}) ", courseModel.CourseName, employeeModel.FullName, employeeModel.EmployeeNo);

                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();
                        }
                        else
                            message = "Course already added.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddInitialOrientation(LMSEmployeeCourse model)
        {
            // Adds Initial Orientation courses to Employee 

            bool isSuccess = false;
            LMSDBContext context = null;
            string message = "";
            try
            {
                if (model != null && model.LMSEmployeeId > 0)
                {
                    context = new LMSDBContext();
                    var emp = context.LMSEmployees.Include("LMSBusinessUnit").SingleOrDefault(x => x.LMSEmployeeId == model.LMSEmployeeId);
                    if (emp != null)
                    {
                        //if (!emp.IsInitialOrientationApplied)
                        //{
                        string businessCourseCode = "";
                        var lmsBU = emp.LMSBusinessUnit;
                        if (lmsBU != null && !string.IsNullOrEmpty(lmsBU.BusinessUnitName))
                            businessCourseCode = lmsBU.BusinessUnitName[0].ToString();
                        var empCourses = context.LMSEmployeeCourses.Include("LMSCourse").Where(x => x.LMSEmployeeId == model.LMSEmployeeId && x.LMSCourse.IsInitialOrientation == true).Select(x => x.LMSCourseId).Distinct().ToList();
                        List<LMSCourse> lstCourse = context.LMSCourses.Where(x => x.StatusCode == StatusCodeConstants.Active && x.IsInitialOrientation == true
                        && x.CourseCode.StartsWith(businessCourseCode)
                        && !empCourses.Contains(x.LMSCourseId)).ToList();
                        if (lstCourse != null && lstCourse.Count > 0)
                        {
                            List<LMSEmployeeCourse> empCourse = new List<LMSEmployeeCourse>();
                            foreach (var course in lstCourse)
                            {
                                empCourse.Add(new LMSEmployeeCourse
                                {
                                    LMSEmployeeId = model.LMSEmployeeId,
                                    LMSCourseId = course.LMSCourseId,
                                    Evaluation = model.Evaluation,
                                    Remarks = model.Remarks,
                                    InstructorName = model.InstructorName,
                                    //PreviousDate = DateTime.Now,
                                    CompletedDate = (model.CompletedDate.HasValue) ? model.CompletedDate : DateTime.Now,
                                    DueDate = (course.Frequency > 0) ? DateTime.Now.AddMonths(course.Frequency) : (DateTime?)null,
                                    //IsInitialOrientationApplied = true,
                                    CreatedBy = LoggedInUser.UserId,
                                    CreatedOn = DateTime.Now,
                                });

                                // add record in LMSCourseHistory Table 

                                LMSCourseHistory courseHistory = new LMSCourseHistory
                                {
                                    LMSEmployeeId = model.LMSEmployeeId,
                                    LMSCourseId = course.LMSCourseId,
                                    CompletedDate = (model.CompletedDate.HasValue) ? model.CompletedDate : DateTime.Now,
                                    StatusCode = StatusCodeConstants.Active,
                                    CreatedOn = DateTime.Now,
                                    CreatedBy = LoggedInUser.UserId
                                };
                                unitofwork.LMSCourseHistoryRepository.Insert(courseHistory);
                            }
                            context.LMSEmployeeCourses.AddRange(empCourse);

                            emp.IsInitialOrientationApplied = true;
                            context.SaveChanges();
                            isSuccess = true;

                            //add record of adding initial orietnation in LMSAudit table

                            var employeeModel = unitofwork.LMSEmployeeRepository.GetByID(model.LMSEmployeeId);

                            string description = String.Format("Added Initial Courses to Employee - {0} (Employee Number - {1}): ", employeeModel.FullName, employeeModel.EmployeeNo);
                            string coursename = "";
                            int count = 1;

                            // list of all the courses in the initial orientation courses added 
                            foreach (var course in lstCourse)
                            {
                                if (count == 1)
                                {
                                    coursename = course.CourseName;
                                    count++;
                                }
                                else
                                {
                                    coursename += ", " + course.CourseName;
                                }

                            }
                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            lmsAudit.Section = "Employee-Course";
                            lmsAudit.Action = "Add - Initial Orietation";
                            //   lmsAudit.Description = description + coursename;
                            lmsAudit.Description = description;

                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();
                        }
                        //}
                        else
                            message = "Initial Orientation Courses already applied.";
                    }
                    else
                        message = "Employee not found.";
                }
            }
            //start here 
            //option1
            //catch (DbEntityValidationException ex)
            //{
            //    // Retrieve the error messages as a list of strings.
            //    var errorMessages = ex.EntityValidationErrors
            //            .SelectMany(x => x.ValidationErrors)
            //            .Select(x => x.ErrorMessage);

            //    // Join the list to a single string.
            //    var fullErrorMessage = string.Join("; ", errorMessages);

            //    // Combine the original exception message with the new one.
            //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
            //    throw;

            //}
            //ends option1
            //option 2 
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
            //ends option2
            //ends here 
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditEmployeeCourse(int id)
        {
            // Return the Employee Course that is edited

            LMSDBContext context = null;
            LMSEmployeeCourse entity = null;
            LMSEmployeeCourse_VM empCourse_VM = null;
            try
            {
                context = new LMSDBContext();
                entity = context.LMSEmployeeCourses.Include("LMSEmployee").Include("LMSCourse")
                    .SingleOrDefault(x => x.LMSEmployeeCourseId == id && x.StatusCode != StatusCodeConstants.InActive);
                if (entity != null)
                {
                    empCourse_VM = new LMSEmployeeCourse_VM
                    {
                        LMSEmployeeCourseId = entity.LMSEmployeeCourseId,
                        LMSEmployeeId = entity.LMSEmployeeId,
                        LMSCourseId = entity.LMSCourseId,
                        EmployeeName = entity.LMSEmployee.FirstName + " " + entity.LMSEmployee.LastName,
                        CourseName = entity.LMSCourse.CourseName,
                        //adding here Course Code
                        CourseCode = entity.LMSCourse.CourseCode,
                        InstructorName = entity.InstructorName,
                        Evaluation = entity.Evaluation,
                        Remarks = entity.Remarks,
                        // Removed Previous Date 11/21/2016
                        //PreviousDate = (entity.PreviousDate.HasValue) ? entity.PreviousDate : (DateTime?)null,
                        PreviousDate = entity.CompletedDate,
                        CompletedDate = entity.CompletedDate,
                        DueDate = entity.DueDate,
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { entity = empCourse_VM }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComputedDueDate(DateTime? CompletedDate, int? Frequency)
        {
            // return the due date for the couses when the employee is due to take the course
            string dueDateInFormat = "";
            DateTime? dueDate = null;
            if (CompletedDate != null && CompletedDate.HasValue && Frequency.HasValue && Frequency.Value > 0)
            {
                dueDate = (Frequency.HasValue && Frequency.Value > 0) ? CompletedDate.Value.AddMonths(Frequency.Value) : (DateTime?)null;
                dueDateInFormat = (dueDate.HasValue) ? dueDate.Value.ToString("MM/dd/yyyy") : "";
            }
            return Json(dueDateInFormat, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddTitleCourses(int employeeId, int jobTitleId)
        {
            // Add the Title Courses (job title courses for the employee) 
            bool isSuccess = false;
            string message = string.Empty;
            try
            {
                var empCourseIds = unitofwork.LMSEmployeeCourseRepository.Get(x => x.LMSEmployeeId == employeeId).Select(x => x.LMSCourseId).ToList();
                var courseIds = unitofwork.LMSJobTitleCourseRepository.Get(x => x.LMSJobTitleId == jobTitleId && !empCourseIds.Contains(x.LMSCourseId)).Select(x => x.LMSCourseId).ToList();
                var courses = unitofwork.LMSCourseRepository.Get(x => courseIds.Contains(x.LMSCourseId)).ToList();
                if (courses != null && courses.Count > 0)
                {
                    var empEntity = unitofwork.LMSEmployeeRepository.GetByID(employeeId);
                    string empName = empEntity.FullName;
                    string description = String.Format("Added Job Title Courses to Employee - {0} (Employee Number - {1}): ", empName, employeeId);
                    string coursename = "";
                    int count = 1;

                    var businessUnit = unitofwork.LMSBusinessUnitRepository.GetByID(empEntity.LMSBusinessUnitId);
                    string businessCourseCode = "";
                    if (businessUnit != null && !string.IsNullOrEmpty(businessUnit.BusinessUnitName))
                        businessCourseCode = businessUnit.BusinessUnitName[0].ToString();

                    foreach (var course in courses.Where(x=>x.CourseCode.StartsWith(businessCourseCode)))
                    {
                        unitofwork.LMSEmployeeCourseRepository.Insert(new LMSEmployeeCourse
                        {
                            LMSEmployeeId = employeeId,
                            LMSCourseId = course.LMSCourseId,
                            //Evaluation = "",
                            //Remarks = "",
                            //InstructorName = "",
                            //CompletedDate = DateTime.Now,
                            DueDate = (course.Frequency > 0) ? DateTime.Now.AddMonths(course.Frequency) : (DateTime?)null,
                            CreatedBy = LoggedInUser.UserId,
                            CreatedOn = DateTime.Now,
                        });

                        // add record in Course History for the title courses taken by employee

                        LMSCourseHistory courseHistory = new LMSCourseHistory
                        {
                            LMSEmployeeId = employeeId,
                            LMSCourseId = course.LMSCourseId,
                            //CompletedDate = DateTime.Now,
                            StatusCode = StatusCodeConstants.Active,
                            CreatedOn = DateTime.Now,
                            CreatedBy = LoggedInUser.UserId
                        };
                        unitofwork.LMSCourseHistoryRepository.Insert(courseHistory);

                        // add record in LMSAudit of the added job title courses to an employee 
                        if (count == 1)
                        {
                            coursename = course.CourseName;
                            count++;
                        }
                        else
                        {
                            coursename += ", " + course.CourseName;
                        }

                    }
                    LMSAudit lmsAudit = new LMSAudit();
                    lmsAudit.TransactionDate = DateTime.Now;
                    lmsAudit.UserName = LoggedInUser.UserName;
                    lmsAudit.FullName = LoggedInUser.FullName;
                    lmsAudit.Section = "Employee-Course";
                    lmsAudit.Action = "Add - Job Title";
                    //lmsAudit.Description = description + coursename;
                    lmsAudit.Description = description;

                    unitofwork.LMSAuditRepository.Insert(lmsAudit);
                    unitofwork.Save();
                    isSuccess = true;
                }
                else
                    message = "No Course is mapped with employee's JobTitle / Course already added.";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTitleCourses(int employeeId, int jobTitleId)
        {
            // returns the job title courses taken by an employee 
            List<LMSEmployeeCourse_VM> lstLMSCourse = null;
            LMSDBContext context = null;
            try
            {
                context = new LMSDBContext();
                var courseIds = unitofwork.LMSEmployeeCourseRepository.Get(x => x.LMSEmployeeId == employeeId).Select(x => x.LMSCourseId).ToList();
                lstLMSCourse = (from p in context.LMSJobTitleCourses.Where(x => x.LMSJobTitleId == jobTitleId)
                                join q in context.LMSCourses.Where(x => !courseIds.Contains(x.LMSCourseId)) on p.LMSCourseId equals q.LMSCourseId
                                select new LMSEmployeeCourse_VM { CourseCode = q.CourseCode, CourseName = q.CourseName }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { lstLMSCourse = lstLMSCourse }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteEmployeeCourse(int id)
        {
            // Deletes a course taken by an employee 

            LMSEmployeeCourse entity = null;
            bool isSuccess = false;
            try
            {
                entity = unitofwork.LMSEmployeeCourseRepository.GetByID(id);
                unitofwork.LMSEmployeeCourseRepository.Delete(entity);
                unitofwork.Save();
                isSuccess = true;

                // adding a record in LMSAudit the course deleted from an employee.

                var employeeEntity = unitofwork.LMSEmployeeRepository.GetByID(entity.LMSEmployeeId);
                var courseEntity = unitofwork.LMSCourseRepository.GetByID(entity.LMSCourseId);

                LMSAudit lmsAudit = new LMSAudit();
                lmsAudit.TransactionDate = DateTime.Now;
                lmsAudit.UserName = LoggedInUser.UserName;
                lmsAudit.FullName = LoggedInUser.FullName;
                lmsAudit.Section = "Employee-Course";
                lmsAudit.Action = "Delete";
                lmsAudit.Description = String.Format("Deleted Course: {0} from {1} (Employee Number: {2}) ", courseEntity.CourseName, employeeEntity.FullName, employeeEntity.EmployeeNo);

                unitofwork.LMSAuditRepository.Insert(lmsAudit);
                unitofwork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}
#endregion