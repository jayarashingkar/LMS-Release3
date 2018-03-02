using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UAC.LMS.Common.Constants;
using UAC.LMS.DAL;
using UAC.LMS.Models;

namespace UAC.LMS.Web.Controllers
{
    /// <summary>
    /// Admin Entities
    /// </summary>
    public class AdminController : BaseController
    {
        UnitOfWork unitofwork = new UnitOfWork();
        #region Admin

        #region JobTitle

        public ActionResult JobTitleList()
        {
            return View();
        }

        public ActionResult SaveJobTitle(LMSJobTitle model)
        {
            bool isSuccess = false;


            string message = "";
            try
            {
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(model.JobTitleName))
                    {
                        if (model.LMSJobTitleId > 0)
                        {
                            var count = unitofwork.LMSJobTitleRepository.Get(x => x.LMSJobTitleId != model.LMSJobTitleId && x.JobTitleName == model.JobTitleName).Count();
                            if (count == 0)
                            {

                                var entity = unitofwork.LMSJobTitleRepository.GetByID(model.LMSJobTitleId);
                                LMSJobTitle oldEntity = new LMSJobTitle();
                                oldEntity.JobTitleName = entity.JobTitleName;

                                //saving the Edited information LMSJobTitle Table
                                entity.JobTitleName = model.JobTitleName;
                                entity.LastModifiedBy = LoggedInUser.UserId;
                                entity.LastModifiedOn = DateTime.Now;
                                unitofwork.LMSJobTitleRepository.Update(entity);
                                unitofwork.Save();
                                isSuccess = true;

                                // saving the Edited information in LMSAudit table
                                LMSAudit lmsAudit = new LMSAudit();
                                lmsAudit.TransactionDate = DateTime.Now;
                                lmsAudit.UserName = LoggedInUser.UserName;
                                lmsAudit.FullName = LoggedInUser.FullName;
                                lmsAudit.Section = "Job Titles";
                                lmsAudit.Action = "Edit";
                                lmsAudit.Description = String.Format("Edited Job Title :{0} to {1}", oldEntity.JobTitleName, model.JobTitleName);
                                unitofwork.LMSAuditRepository.Insert(lmsAudit);
                                unitofwork.Save();
                            }
                            else
                                message = "Title Name already exists.";
                        }
                        else
                        {
                            var count = unitofwork.LMSJobTitleRepository.Get(x => x.JobTitleName == model.JobTitleName).Count();
                            if (count == 0)
                            {
                                //saving the new Job Title LMSJobTitle Table
                                model.CreatedBy = LoggedInUser.UserId;
                                model.CreatedOn = DateTime.Now;
                                model.StatusCode = StatusCodeConstants.Active;
                                unitofwork.LMSJobTitleRepository.Insert(model);
                                unitofwork.Save();
                                isSuccess = true;

                                //saving the new Job Title LMSAudit Table
                                LMSAudit lmsAudit = new LMSAudit();
                                lmsAudit.TransactionDate = DateTime.Now;
                                lmsAudit.UserName = LoggedInUser.UserName;
                                lmsAudit.FullName = LoggedInUser.FullName;
                                lmsAudit.Section = "Job Titles";
                                lmsAudit.Action = "Add";
                                lmsAudit.Description = String.Format("Added new Job Title: {0}", model.JobTitleName);
                                unitofwork.LMSAuditRepository.Insert(lmsAudit);
                                unitofwork.Save();
                            }
                            else
                                message = "Title Name already exists.";
                        }
                    }
                    else
                        message = "Title Name is required.";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }         
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }
        
           
        public ActionResult EditJobTitle(int id)
        {
            /// This function returns the Job Title entity that needs to be Edited

            LMSJobTitle entity = null;
            try
            {
                entity = unitofwork.LMSJobTitleRepository.GetByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
            return Json(new { entity = entity }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteJobTitle(int id)
        {
            LMSJobTitle entity = null;
            bool isSuccess = false;
            int count = 0;

            string message = string.Empty;
            try
            {
                count = unitofwork.LMSEmployeeRepository.Get(x => x.LMSJobTitleId == id).Count();
                if (count == 0)
                {
                    //Deletes the Job title from LMSJobTitle
                    entity = unitofwork.LMSJobTitleRepository.GetByID(id);
                    unitofwork.LMSJobTitleRepository.Delete(entity);
                    unitofwork.Save();
                    isSuccess = true;

                    // Saves the Record of deleted Job Title in LMSAudit
                    LMSAudit lmsAudit = new LMSAudit();
                    lmsAudit.TransactionDate = DateTime.Now;
                    lmsAudit.UserName = LoggedInUser.UserName;
                    lmsAudit.FullName = LoggedInUser.FullName;
                    lmsAudit.Section = "Job Titles";
                    lmsAudit.Action = "Delete";
                    lmsAudit.Description = String.Format("Deleted Job Title :{0} ", entity.JobTitleName);
                    unitofwork.LMSAuditRepository.Insert(lmsAudit);
                    unitofwork.Save();
                }
                else
                {
                    message = "Job Title is assigned to employee. So you can't delete it.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Courses

        public ActionResult CourseList()
        {
            try
            {               
                //sort job Titles in alphabetical order and store in ViewBag.ddJobTitle to access from front end
                IEnumerable<LMSJobTitle> jobTitles = unitofwork.LMSJobTitleRepository.Get(x => x.StatusCode == StatusCodeConstants.Active).OrderBy(j=>j.JobTitleName);
                List<SelectListItem> ddJobTitles = new List<SelectListItem> { new SelectListItem { Text = "Please Select", Value = "-1" } };
                jobTitles.ToList().ForEach(x => ddJobTitles.Add(new SelectListItem { Text = x.JobTitleName, Value = x.LMSJobTitleId.ToString(), Selected = false }));
                ViewBag.ddJobTitles = ddJobTitles;
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SaveCourse(LMSCourse model, List<int> LMSJobTitleIds)
        {
            bool isSuccess = false;
            string message = string.Empty;
            try
            {
                if (model != null)
                {
                    //added here to store Course Code in Course Name
                    model.CourseName = model.CourseName + " (" + model.CourseCode + ")";
                    if (model.LMSCourseId > 0)
                    {
                        var nameCount = unitofwork.LMSCourseRepository.Get(x => x.LMSCourseId != model.LMSCourseId && x.CourseName == model.CourseName).Count();
                        if (nameCount > 0)
                            message = "Course Name already exists.";
                        var codeCount = unitofwork.LMSCourseRepository.Get(x => x.LMSCourseId != model.LMSCourseId && x.CourseCode == model.CourseCode).Count();
                        if (codeCount > 0)
                            message = "Course Code already exists.";
                        if (nameCount == 0 && codeCount == 0)
                        {
                            var entity = unitofwork.LMSCourseRepository.GetByID(model.LMSCourseId);

                            LMSCourse oldEntity = new LMSCourse();
                            oldEntity.CourseName = entity.CourseName;
                            oldEntity.CourseCode = entity.CourseCode;
                            oldEntity.CourseLength = entity.CourseLength;
                            oldEntity.Frequency = entity.Frequency;
                            oldEntity.IsReocurring = entity.IsReocurring;
                            oldEntity.IsInitialOrientation = entity.IsInitialOrientation;

                            //save edited Course information in LMSCourse table
                            entity.CourseName = model.CourseName;//+ " (" + model.CourseCode + ")" ;
                            entity.CourseCode = model.CourseCode;
                            entity.CourseLength = model.CourseLength;
                            entity.Frequency = model.Frequency;
                            entity.IsReocurring = model.IsReocurring;
                            entity.IsInitialOrientation = model.IsInitialOrientation;
                            entity.LastModifiedBy = LoggedInUser.UserId;
                            entity.LastModifiedOn = DateTime.Now;
                            unitofwork.LMSCourseRepository.Update(entity);
                            unitofwork.Save();
                            isSuccess = true;

                            //saving a record of Edited Course information in LMSAudit table

                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            lmsAudit.Section = "Course";
                            lmsAudit.Action = "Edit";
                            lmsAudit.Description = String.Format("Edited Course: {0}", oldEntity.CourseName);

                            if (oldEntity.CourseName != model.CourseName)
                                lmsAudit.Description += String.Format(" * Edited Course Name: {0} to {1}", oldEntity.CourseName, model.CourseName);
                            if (oldEntity.CourseCode != model.CourseCode)
                                lmsAudit.Description += String.Format(" * Edited Course Code: {0} to {1}", oldEntity.CourseCode, model.CourseCode);

                            // Changed field name CourseLength to Training Time                           

                            if (oldEntity.CourseLength != model.CourseLength)
                            {
                                if (oldEntity.CourseLength == null)
                                {
                                    oldEntity.CourseLength = "Empty";
                                    lmsAudit.Description += String.Format(" * Edited Training Time: {0} to {1}", oldEntity.CourseLength, model.CourseLength);
                                    oldEntity.CourseLength = null;
                                }
                               else if (model.CourseLength == null)
                                {
                                    model.CourseLength = "Empty";
                                    lmsAudit.Description += String.Format(" * Edited Training Time: {0} to {1}", oldEntity.CourseLength, model.CourseLength);
                                    model.CourseLength = null;
                                }
                                else
                                    if ((oldEntity.CourseLength != null) && (model.CourseLength != null))
                                        lmsAudit.Description += String.Format(" * Edited Training Time: {0} to {1}", oldEntity.CourseLength, model.CourseLength);
                            }
                            if (oldEntity.Frequency != model.Frequency)
                                lmsAudit.Description += String.Format(" * Edited Frequency: {0} to {1}", oldEntity.Frequency, model.Frequency);
                            if (oldEntity.IsReocurring != model.IsReocurring)
                                lmsAudit.Description += String.Format(" * Edited Reocurring: {0} to {1}", oldEntity.IsReocurring, model.IsReocurring);
                            if (oldEntity.IsInitialOrientation != model.IsInitialOrientation)
                                lmsAudit.Description += String.Format(" * Edited Initial Orientation: {0} to {1}", oldEntity.IsInitialOrientation, model.IsInitialOrientation);

                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();

                        }
                       if (LMSJobTitleIds != null)
                        {                            
                            var oldMapping = unitofwork.LMSJobTitleCourseRepository.Get(x => x.LMSCourseId == model.LMSCourseId &&
                            !LMSJobTitleIds.Contains(x.LMSJobTitleId)).ToList();
                            if (oldMapping != null && oldMapping.Count > 0)
                            {
                                foreach (var old in oldMapping)
                                {
                                    // during Editing a Course - code to remove a Job from a Course
                                    int tId = old.LMSJobTitleCourseId;
                                    int jtId = old.LMSJobTitleId;
                                    var jtEntity = unitofwork.LMSJobTitleRepository.GetByID(jtId);
                                    var jtName = jtEntity.JobTitleName;
                                    
                                    unitofwork.LMSJobTitleCourseRepository.Delete(old);

                                    LMSAudit lmsAudit = new LMSAudit();
                                    lmsAudit.TransactionDate = DateTime.Now;
                                    lmsAudit.UserName = LoggedInUser.UserName;
                                    lmsAudit.FullName = LoggedInUser.FullName;
                                    lmsAudit.Section = "TitleCourse";
                                                               
                                    lmsAudit.Action = "Edit";
                                    lmsAudit.Description = String.Format("Removed Job Title: '{0}' from Course: {1} (Course Code: {2}) ", jtName, model.CourseName, model.CourseCode);
                                  
                                    unitofwork.LMSAuditRepository.Insert(lmsAudit);

                                }
                                unitofwork.Save();
                            }

                            // Add Job Title to course

                            foreach (var tId in LMSJobTitleIds)
                            {
                                if (tId != -1 && unitofwork.LMSJobTitleCourseRepository.Get(x => x.LMSCourseId == model.LMSCourseId && x.LMSJobTitleId == tId).Count() == 0)
                                {
                                    var jtEntity = unitofwork.LMSJobTitleRepository.GetByID(tId);
                                    var jtName = jtEntity.JobTitleName;

                                    //saving job title to a course in LMSJobTitleCourse table

                                    LMSJobTitleCourse titleCourse = new LMSJobTitleCourse
                                    {
                                        LMSJobTitleId = tId,
                                        LMSCourseId = model.LMSCourseId,
                                        StatusCode = StatusCodeConstants.Active,
                                        CreatedBy = LoggedInUser.UserId,
                                        CreatedOn = DateTime.Now
                                    };
                                    unitofwork.LMSJobTitleCourseRepository.Insert(titleCourse);

                                    //saving record in LMSAudit table
                                    LMSAudit lmsAudit = new LMSAudit();
                                    lmsAudit.TransactionDate = DateTime.Now;
                                    lmsAudit.UserName = LoggedInUser.UserName;
                                    lmsAudit.FullName = LoggedInUser.FullName;
                                    lmsAudit.Section = "TitleCourse";
                                    lmsAudit.Action = "Edit";
                                    lmsAudit.Description = String.Format("Added new Job Title:{0} to Course: {1} (course Code: {2}) ", jtName, model.CourseName, model.CourseCode);
                                    unitofwork.LMSAuditRepository.Insert(lmsAudit);
                                }
                            }
                            unitofwork.Save();
                        }
                        else
                        {
                            // Deleted the old record from repository - after saving the new while editing
                            var oldMapping = unitofwork.LMSJobTitleCourseRepository.Get(x => x.LMSCourseId == model.LMSCourseId).ToList();
                            if (oldMapping != null)
                            {
                                foreach (var old in oldMapping)
                                {
                                    unitofwork.LMSJobTitleCourseRepository.Delete(old);
                                }
                            }
                            unitofwork.Save();
                        }
                    }
                    else
                    {
                        //Adding new Course

                        var nameCount = unitofwork.LMSCourseRepository.Get(x => x.CourseName == model.CourseName).Count();
                        if (nameCount > 0)
                            message = "Course Name already exists.";
                        var codeCount = unitofwork.LMSCourseRepository.Get(x => x.CourseCode == model.CourseCode).Count();
                        if (codeCount > 0)
                            message = "Course Code already exists.";
                        if (nameCount == 0 && codeCount == 0)
                        {
                            //Saving a new Course in LMSCourse table
                            model.StatusCode = StatusCodeConstants.Active;
                            model.CreatedBy = LoggedInUser.UserId;
                            model.CreatedOn = DateTime.Now;
                           
                            unitofwork.LMSCourseRepository.Insert(model);
                            unitofwork.Save();
                            isSuccess = true;

                            //Saving a record of new Course added in LMSAudit table
                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            lmsAudit.Section = "Course";
                            lmsAudit.Action = "Add";
                            lmsAudit.Description = String.Format("Added new Course: {0}, {1} ", model.CourseCode, model.CourseName);
                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();
                        }
                        if (LMSJobTitleIds != null && LMSJobTitleIds.Count > 0)
                        {

                            // adding new title to a newly added course 

                            string description = String.Format("New Course - {0} (Course Code: {1}) is added to Job titles: ", model.CourseName, model.CourseCode);
                            int count = 1;
                            foreach (var tId in LMSJobTitleIds)
                            {                                
                                if (tId != -1)
                                {
                                    var jtEntity = unitofwork.LMSJobTitleRepository.GetByID(tId);
                                    var jtName = jtEntity.JobTitleName;

                                    LMSJobTitleCourse titleCourse = new LMSJobTitleCourse
                                    {
                                        LMSJobTitleId = tId,
                                        LMSCourseId = model.LMSCourseId,
                                        StatusCode = StatusCodeConstants.Active,
                                        CreatedBy = LoggedInUser.UserId,
                                        CreatedOn = DateTime.Now
                                    };
                                    unitofwork.LMSJobTitleCourseRepository.Insert(titleCourse);

                                       if (count == 1)
                                    {                                       
                                        description += jtName;
                                        count++;
                                    }
                                    else
                                    {
                                        jtName = ", " + jtName;
                                        description += jtName;
                                    }                                  
                                }
                            }

                            //saving a record in LMSAudit table 

                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            lmsAudit.Section = "TitleCourse";
                            lmsAudit.Action = "Add";
                            lmsAudit.Description = description;

                            unitofwork.LMSAuditRepository.Insert(lmsAudit);

                            unitofwork.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditCourse(int id)
        {
            //returns the entity from the LMSCourse that needs to be Edited
            LMSCourse entity = null;
            LMSDBContext context = null;
            List<int> jobTitleIds = null;
            try
            {
                entity = unitofwork.LMSCourseRepository.GetByID(id);
                context = new LMSDBContext();
                // list of job titles the course is assigned to
                jobTitleIds = context.LMSJobTitleCourses.Where(x => x.LMSCourseId == id).Select(x => x.LMSJobTitleId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { entity = entity, jobTitleIds = jobTitleIds }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCourse(int id)
        {
            // Deletes the Course from LMSCourse table

            LMSCourse entity = null;
            LMSDBContext context = null;
            bool isSuccess = false;
            int count = 0;
            string message = string.Empty;
            try
            {
                context = new LMSDBContext();
                count = context.LMSEmployeeCourses.Include("LMSCourse").Count(x => x.LMSCourseId == id);
                if (count == 0)
                {
                    entity = unitofwork.LMSCourseRepository.GetByID(id);
                    unitofwork.LMSCourseRepository.Delete(entity);
                    unitofwork.Save();
                    isSuccess = true;

                    // saves the record of Deleted Course in LMSAudit table
                    LMSAudit lmsAudit = new LMSAudit();
                    lmsAudit.TransactionDate = DateTime.Now;
                    lmsAudit.UserName = LoggedInUser.UserName;
                    lmsAudit.FullName = LoggedInUser.FullName;
                    lmsAudit.Section = "Course";
                    lmsAudit.Action = "Delete";
                    lmsAudit.Description = string.Format("Deleted Course: {0}, {1}", entity.CourseCode, entity.CourseName);
                    unitofwork.LMSAuditRepository.Insert(lmsAudit);
                    unitofwork.Save();
                }
                else
                {
                    message = "Course is assigned to employee. So you can't delete it.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region BusinessUnits

        public ActionResult BusinessUnitList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveBusinessUnit(LMSBusinessUnit model)
        {
            bool isSuccess = false;
            string message = string.Empty;
            try
            {
                if (model != null)
                {
                    if (model.LMSBusinessUnitId > 0)
                    {
                        var count = unitofwork.LMSBusinessUnitRepository.Get(x => x.LMSBusinessUnitId != model.LMSBusinessUnitId &&
                        x.BusinessUnitName == model.BusinessUnitName).Count();
                        if (count == 0)
                        {
                            var entity = unitofwork.LMSBusinessUnitRepository.GetByID(model.LMSBusinessUnitId);

                            // save the Edited Business Unit / Location

                            LMSBusinessUnit oldEntity = new LMSBusinessUnit();
                            oldEntity.BusinessUnitName = entity.BusinessUnitName;

                            entity.BusinessUnitName = model.BusinessUnitName;
                            entity.LastModifiedBy = LoggedInUser.UserId;
                            entity.LastModifiedOn = DateTime.Now;
                            unitofwork.LMSBusinessUnitRepository.Update(entity);
                            unitofwork.Save();
                            isSuccess = true;

                            //saves a record of Saved Business unit in LMSAudit table

                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;

                            // Changed  Business Unit to  Location 11/21/2016
                            //lmsAudit.Section = "Business Unit";

                            lmsAudit.Section = "Location";
                            lmsAudit.Action = "Edit";
                            // Changed  Business Unit to  Location 11/21/2016
                            //lmsAudit.Description = String.Format("Edited Business Unit Name: {0} to {1}", oldEntity.BusinessUnitName, model.BusinessUnitName);

                            lmsAudit.Description = String.Format("Edited Location Name: {0} to {1}", oldEntity.BusinessUnitName, model.BusinessUnitName);
                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();

                        }
                        else
                            // Changed  Business Unit to  Location 11/21/2016
                           // message = "Business Unit Name already exsists.";
                            message = "Location Name already exsists.";
                    }
                    else
                    {
                        //save new Business Unit / Location

                        var count = unitofwork.LMSBusinessUnitRepository.Get(x => x.BusinessUnitName == model.BusinessUnitName).Count();
                        if (count == 0)
                        {
                            model.StatusCode = StatusCodeConstants.Active;
                            model.CreatedBy = LoggedInUser.UserId;
                            model.CreatedOn = DateTime.Now;
                            unitofwork.LMSBusinessUnitRepository.Insert(model);
                            unitofwork.Save();
                            isSuccess = true;

                            // save a record of new Business Unit/ Location added in LMSAudit table

                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            // Changed  Business Unit to  Location 11/21/2016
                            //lmsAudit.Section = "Business Unit";
                            lmsAudit.Section = "Location";
                            lmsAudit.Action = "Add";
                            //lmsAudit.Description = String.Format("Added new Business Unit: {0}", model.BusinessUnitName);
                            lmsAudit.Description = String.Format("Added new Location: {0}", model.BusinessUnitName);
                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();
                        }
                        else
                            // message = "Business Unit Name already exsists.";
                            message = "Location Name already exsists.";
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditBusinessUnit(int id)
        {
            //return the Business Unit/ Location that needs to be Edited
            LMSBusinessUnit entity = null;
            try
            {
                entity = unitofwork.LMSBusinessUnitRepository.GetByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { entity = entity }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteLMSBusinessUnit(int id)
        {
            //Deletes the Location / Business Unit
            LMSBusinessUnit entity = null;
            bool isSuccess = false;
            int count = 0;
            LMSDBContext context = null;
            string message = string.Empty;
            try
            {
                context = new LMSDBContext();
                count = context.LMSEmployees.Count(x => x.StatusCode == StatusCodeConstants.Active
                 && x.LMSBusinessUnitId == id);
                if (count == 0)
                {
                    entity = unitofwork.LMSBusinessUnitRepository.GetByID(id);
                    unitofwork.LMSBusinessUnitRepository.Delete(entity);
                    unitofwork.Save();
                    isSuccess = true;

                    // saves a record in LMSAudit table of the deleted Business Unit / Location
                    LMSAudit lmsAudit = new LMSAudit();
                    lmsAudit.TransactionDate = DateTime.Now;
                    lmsAudit.UserName = LoggedInUser.UserName;
                    lmsAudit.FullName = LoggedInUser.FullName;
                    // Changed  Business Unit to  Location 11/21/2016
                    //lmsAudit.Section = "Business Unit";
                    lmsAudit.Section = "Location";
                    lmsAudit.Action = "Delete";
                    // lmsAudit.Description = String.Format("Deleted Business Unit : {0} ", entity.BusinessUnitName);
                    lmsAudit.Description = String.Format("Deleted Location : {0} ", entity.BusinessUnitName);
                    unitofwork.LMSAuditRepository.Insert(lmsAudit);
                    unitofwork.Save();

                }
                else
                {
                    //  message = "Business Unit is assigned to employee. So you can't delete it.";
                    message = "Location is assigned to employee. So you can't delete it.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Departments

        public ActionResult DepartmentList()
        {
            // comment here 
            try
            {                            
                // sort and store list of Business Units / Locations in ViewBag.ddBusinessUnits 

                IEnumerable<LMSBusinessUnit> businessUnits = unitofwork.LMSBusinessUnitRepository.Get(x => x.StatusCode == StatusCodeConstants.Active).OrderBy(j => j.BusinessUnitName);
                List<SelectListItem> ddBusinessUnits = new List<SelectListItem> { new SelectListItem { Text = "Please Select", Value = "-1" } };
              //  IEnumerable<LMSBusinessUnit> businessUnitsQuery = businessUnits.OrderBy(j => j.BusinessUnitName);
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
        public ActionResult SaveDepartment(LMSDepartment model)
        {
            bool isSuccess = false;
            string message = string.Empty;
            try
            {
                if (model != null)
                {
                    //added here to store Course Code in Course Name
                    model.DepartmentName = model.DepartmentName + " (" + model.DepartmentCode + ")";
                    if (model.LMSDepartmentId > 0)
                    {
                        // Save the Edited Department information 
                        //nameCount to get Unique Department Names
                        var nameCount = unitofwork.LMSDepartmentRepository.Get(x => x.LMSDepartmentId != model.LMSDepartmentId && x.DepartmentName == model.DepartmentName).Count();
                        if (nameCount > 0)
                            message = "Department Name already exists.";
                        //codeCount to get Unique Department Names
                        var codeCount = unitofwork.LMSDepartmentRepository.Get(x => x.LMSDepartmentId != model.LMSDepartmentId && x.DepartmentCode == model.DepartmentCode).Count();
                        if (codeCount > 0)
                            message = "Department Code already exists.";
                        if (nameCount == 0 && codeCount == 0)
                        {
                            var entity = unitofwork.LMSDepartmentRepository.GetByID(model.LMSDepartmentId);
                          
                            LMSDepartment oldEntity = new LMSDepartment();
                            oldEntity.DepartmentCode = entity.DepartmentCode;
                            oldEntity.DepartmentName = entity.DepartmentName;

                            //saving Edited information in Department Table

                            entity.DepartmentName = model.DepartmentName;
                            entity.DepartmentCode = model.DepartmentCode;
                            entity.LastModifiedBy = LoggedInUser.UserId;
                            entity.LastModifiedOn = DateTime.Now;
                            unitofwork.LMSDepartmentRepository.Update(entity);
                            unitofwork.Save();
                            isSuccess = true;

                            //adding the record of Edited Department Table information in Audit Table

                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            lmsAudit.Section = "Department";
                            lmsAudit.Action = "Edit";
                            lmsAudit.Description = String.Format("Edited Department:{0}", oldEntity.DepartmentName);
                            if (oldEntity.DepartmentName != model.DepartmentName)
                                lmsAudit.Description += String.Format(" * Edited Department Name: {0} to {1}", oldEntity.DepartmentName, model.DepartmentName);

                            if (oldEntity.DepartmentCode != model.DepartmentCode)
                                lmsAudit.Description += String.Format(" * Edited Department Code: {0} to {1}", oldEntity.DepartmentCode, model.DepartmentCode);

                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();

                        }
                    }
                    else
                    {
                        //adding new Department

                        var nameCount = unitofwork.LMSDepartmentRepository.Get(x => x.DepartmentName == model.DepartmentName).Count();
                        if (nameCount > 0)
                            message = "Department Name already exists.";
                        var codeCount = unitofwork.LMSDepartmentRepository.Get(x => x.DepartmentCode == model.DepartmentCode).Count();
                        if (codeCount > 0)
                            message = "Department Code already exists.";
                        if (nameCount == 0 && codeCount == 0)
                        {
                            model.StatusCode = StatusCodeConstants.Active;
                            model.CreatedBy = LoggedInUser.UserId;
                            model.CreatedOn = DateTime.Now;
                            unitofwork.LMSDepartmentRepository.Insert(model);
                            unitofwork.Save();
                            isSuccess = true;

                            // adding a record in LMSAudit table 
                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            lmsAudit.Section = "Department";
                            lmsAudit.Action = "Add";
                            lmsAudit.Description = String.Format("Added new Department - Name: {0}, Code: {1}", model.DepartmentName, model.DepartmentCode);
                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditDepartment(int id)
        {
            //return the Department that needs to be Edited
            LMSDepartment entity = null;
            try
            {
                entity = unitofwork.LMSDepartmentRepository.GetByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { entity = entity }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDepartment(int id)
        {
            // Deletes Department
            LMSDepartment entity = null;
            bool isSuccess = false;
            int count = 0;
            string message = string.Empty;
            try
            {
                count = unitofwork.LMSEmployeeRepository.Get(x => x.LMSDepartmentId == id).Count();
                if (count == 0)
                {
                   
                    entity = unitofwork.LMSDepartmentRepository.GetByID(id);
                    unitofwork.LMSDepartmentRepository.Delete(entity);
                    unitofwork.Save();
                    isSuccess = true;

                    // adding record in LMSAudit of Deleted Department
                    LMSAudit lmsAudit = new LMSAudit();
                    lmsAudit.TransactionDate = DateTime.Now;
                    lmsAudit.UserName = LoggedInUser.UserName;
                    lmsAudit.FullName = LoggedInUser.FullName;
                    lmsAudit.Section = "Department";
                    lmsAudit.Action = "Delete";
                    lmsAudit.Description = String.Format("Deleted Department - Name: {0}, Code: {1} ", entity.DepartmentName, entity.DepartmentCode);
                    unitofwork.LMSAuditRepository.Insert(lmsAudit);
                    unitofwork.Save();
                }
                else
                {
                    message = "Department is assigned to employee. So you can't delete it.";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region UserLogins

        public ActionResult UserLoginList()
        {
            // adding List of Permission levels in ViewBag.ddPermissionLevel to access from Front end
            SelectListItem defaultItem = null;
            try
            {
                defaultItem = new SelectListItem { Text = "Please Select", Value = "-1" };
                List<SelectListItem> ddPermissionLevel = new List<SelectListItem>();
                ddPermissionLevel.Add(defaultItem);
                // Added super admin 11/21/2016
                //ddPermissionLevel.Add(new SelectListItem { Text = "All", Value = PermissionConstants.All });           

                ddPermissionLevel.Add(new SelectListItem { Text = "Read Only", Value = PermissionConstants.ReadOnly });
                ddPermissionLevel.Add(new SelectListItem { Text = "Admin", Value = PermissionConstants.Admin });              
                ddPermissionLevel.Add(new SelectListItem { Text = "Super Admin", Value = PermissionConstants.SuperAdmin });

                ViewBag.ddPermissionLevel = ddPermissionLevel;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return View();
        }

        [HttpPost]
        public ActionResult SaveUserLogin(LMSLogin model)
        {
            bool isSuccess = false;
            bool isExists = false;
            LMSLogin lmsLogin = null;
            string message = string.Empty;
            try
            {
                if (model != null)
                {
                    if (model.UserId > 0)
                    {
                        var entity = unitofwork.LMSLoginRepository.GetByID(model.UserId);

                        //save the Edited Login information 

                        LMSLogin oldEntity = new LMSLogin();

                        oldEntity.UserName = entity.UserName;
                        oldEntity.FirstName = entity.FirstName;
                        oldEntity.LastName = entity.LastName;
                        // oldEntity.EmployeeNo = entity.EmployeeNo;
                        //  oldEntity.Email = entity.Email;
                        oldEntity.PermissionLevel = entity.PermissionLevel;
                        oldEntity.IssueDate = entity.IssueDate;

                        entity.FirstName = model.FirstName;
                        entity.LastName = model.LastName;
                        // entity.EmployeeNo = model.EmployeeNo;
                        //  entity.Email = model.Email;
                        entity.PermissionLevel = model.PermissionLevel;
                        entity.IssueDate = model.IssueDate;
                        entity.LastModifiedBy = LoggedInUser.UserId;
                        entity.LastModifiedOn = DateTime.Now;

                        unitofwork.LMSLoginRepository.Update(entity);
                        unitofwork.Save();
                        isSuccess = true;

                        // add a record in LMSAudit, of Editing Login Permission
                        LMSAudit lmsAudit = new LMSAudit();
                        lmsAudit.TransactionDate = DateTime.Now;
                        lmsAudit.UserName = LoggedInUser.UserName;
                        lmsAudit.FullName = LoggedInUser.FullName;
                        lmsAudit.Section = "Login Permissions";
                        lmsAudit.Action = "Edit";
                        lmsAudit.Description = String.Format("Edited Login Permissions for {0}, {1} {2}:", oldEntity.UserName, oldEntity.FirstName, oldEntity.LastName);

                        if (oldEntity.FirstName != model.FirstName)
                            lmsAudit.Description += String.Format(" * Edited First Name: {0} to {1}", oldEntity.FirstName, model.FirstName);
                        if (oldEntity.LastName != model.LastName)
                            lmsAudit.Description += String.Format(" * Edited Last Name: {0} to {1}", oldEntity.LastName, model.LastName);
                        //if (oldEntity.EmployeeNo != model.EmployeeNo)
                        //    lmsAudit.Description += String.Format(" - Edited Employee Number: {0} to {1}", oldEntity.EmployeeNo, model.EmployeeNo);
                        //if (oldEntity.Email != model.Email)
                        //    lmsAudit.Description += String.Format(" - Edited Email: {0} to {1}",oldEntity.Email, model.Email);
                        if (oldEntity.PermissionLevel != model.PermissionLevel)
                            lmsAudit.Description += String.Format(" * Edited Permission Level: {0} to {1}", oldEntity.PermissionLevel, model.PermissionLevel);
                        if (oldEntity.IssueDate != model.IssueDate)
                            lmsAudit.Description += String.Format(" * Edited Issue Date: {0} to {1}", oldEntity.IssueDate, model.IssueDate);

                        unitofwork.LMSAuditRepository.Insert(lmsAudit);
                        unitofwork.Save();
                    }
                    else
                    {
                        //add new Login entry in LMSLogin
                        isExists = unitofwork.LMSLoginRepository.CheckIfUserExists(model.UserName);
                        if (!isExists)
                        {
                            lmsLogin = new LMSLogin
                            {
                                UserName = model.UserName,
                                Password = model.Password,
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                //EmployeeNo = model.EmployeeNo,
                                //Email = model.Email,
                                StatusCode = StatusCodeConstants.Active,
                                IssueDate = model.IssueDate,
                                PermissionLevel = model.PermissionLevel,
                                UserType = model.UserType,
                                CreatedBy = LoggedInUser.UserId,
                                CreatedOn = DateTime.Now,
                            };
                            unitofwork.LMSLoginRepository.Register(lmsLogin);
                            unitofwork.Save();
                            isSuccess = true;

                            // add a record in LMSAudit of Adding a new User Login 
                            LMSAudit lmsAudit = new LMSAudit();
                            lmsAudit.TransactionDate = DateTime.Now;
                            lmsAudit.UserName = LoggedInUser.UserName;
                            lmsAudit.FullName = LoggedInUser.FullName;
                            lmsAudit.Section = "Login Permissions";
                            lmsAudit.Action = "Add";
                            lmsAudit.Description = String.Format("Added new Login Permissions : {0}, {1} {2}", model.UserName, model.FirstName, model.LastName);
                            unitofwork.LMSAuditRepository.Insert(lmsAudit);
                            unitofwork.Save();

                        }
                        else
                            message = "User Name already exists.Try different name.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUserLogin(int id)
        {
            // return the entity of User Login that needs to be Edited
            LMSLogin entity = null;
            try
            {
                entity = unitofwork.LMSLoginRepository.GetByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { entity = entity }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteUserLogins(int id)
        {
            // Delete a User Login from LMSLogin table
            LMSLogin entity = null;
            bool isSuccess = false;
            LMSDBContext context = null;
            try
            {
                context = new LMSDBContext();
                entity = unitofwork.LMSLoginRepository.GetByID(id);
                unitofwork.LMSLoginRepository.Delete(entity);
                unitofwork.Save();
                isSuccess = true;

                // add a record LMSAudit of deleting a User Login 

                LMSAudit lmsAudit = new LMSAudit();
                lmsAudit.TransactionDate = DateTime.Now;
                lmsAudit.UserName = LoggedInUser.UserName;
                lmsAudit.FullName = LoggedInUser.FullName;
                lmsAudit.Section = "Login Permissions";
                lmsAudit.Action = "Delete";
                lmsAudit.Description = String.Format("Deleted Login Permissions :{0}, {1} {2}", entity.UserName, entity.FirstName, entity.LastName);
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
        #region SecuityConfig

        [AllowAnonymous]
        public ActionResult SecuityConfig()
        {
            
            bool IsSecurityApplied = LoggedInUser.IsSecurityApplied;

            // store list of Security question in  ViewBag.ddSecurityQuestions to access from Front End

            IEnumerable<LMSSecurityQuestion> securityQuestions = unitofwork.LMSSecurityQuestionRepository.Get(x => x.StatusCode == StatusCodeConstants.Active);
            List<SelectListItem> ddSecurityQuestions = new List<SelectListItem> { new SelectListItem { Text = "Please Select", Value = "-1" } };
            securityQuestions.ToList().ForEach(x => ddSecurityQuestions.Add(new SelectListItem { Text = x.Question, Value = x.LMSSecurityQuestionId.ToString() }));
            ViewBag.ddSecurityQuestions = ddSecurityQuestions;

            // check if there is corresponding answer to the secruity Question is stored and store in ViewBag.HasQuestionId to access from front end

            LMSDBContext context = new LMSDBContext();
            bool hasQuestionId = false;
            int count = context.LMSUserSecurityAnswers.Count(x => x.LMSLoginId == LoggedInUser.UserId);
            if (count > 0)
                hasQuestionId = true;
            ViewBag.HasQuestionId = hasQuestionId;

            // returns if the Security question is assigned to the Logged in user.
            return View(IsSecurityApplied);
        }

        [HttpPost]
        public ActionResult SecuityConfig(LMSUserSecurityAnswer model)
        {
            bool isSuccess = false;
            string message = "";
            LMSLogin login = null;
            LMSUserSecurityAnswer answer = null;
            try
            {
                login = unitofwork.LMSLoginRepository.GetByID(LoggedInUser.UserId);
                if (login != null)
                {
                    if (model != null)
                    {
                        if (!model.HasQuestionId)
                        {
                            // save security answer for security Question - during the change of password
                            answer = new LMSUserSecurityAnswer
                            {
                                LMSLoginId = LoggedInUser.UserId,
                                LMSSecurityQuestionId = model.LMSSecurityQuestionId,
                                SecurityAnswer = model.SecurityAnswer,
                                StatusCode = StatusCodeConstants.Active,
                                CreatedBy = LoggedInUser.UserId,
                                CreatedOn = DateTime.Now
                            };
                            unitofwork.LMSUserSecurityAnswerRepository.Insert(answer);
                        }

                        //save the password

                        login.IsSecurityApplied = true;
                        login.Password = model.Password;
                        unitofwork.LMSLoginRepository.ChangePassword(login);
                        unitofwork.Save();

                        Response.Cookies["LMSLogin"].Expires = DateTime.Now.AddDays(-1);

                        CurrentUser newUser = LoggedInUser;
                        newUser.IsSecurityApplied = true;
                        Session["CurrentUser"] = newUser;

                        HttpCookie cookie = new HttpCookie("LMSLogin");
                        cookie.Values.Add("UserName", LoggedInUser.UserName);
                        //cookie.Values.Add("Password", model.Password);
                        cookie.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(cookie);
                        isSuccess = true;
                        //return RedirectToAction("EmployeeList", "Employee");

                        //adding here - 11/10/2016
                        ViewBag.ddAuditDescription = LoggedInUser.UserName;

                        //adding here - 1/10/2016
                        // adding a record in LMSAudit table 
                        LMSAudit lmsAudit = new LMSAudit();
                        lmsAudit.TransactionDate = DateTime.Now;
                        lmsAudit.UserName = LoggedInUser.UserName;
                        lmsAudit.FullName = LoggedInUser.FullName;
                        lmsAudit.Section = "Login";
                        lmsAudit.Action = "Password Change";
                        lmsAudit.Description = String.Format("{0} changed Password", LoggedInUser.UserName);
                        unitofwork.LMSAuditRepository.Insert(lmsAudit);
                        unitofwork.Save();

                    }
                }
                else
                    message = "Login details not found.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { isSuccess = isSuccess, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region PrintRoster

        public ActionResult PrintRoster()
        {
            SelectListItem defaultItem = null;
            try
            {
                defaultItem = new SelectListItem { Text = "Please Select", Value = "-1" };

                // sort and store Business Units in  ViewBag.ddBusinessUnits to access from front end

                IEnumerable<LMSBusinessUnit> businessUnits = unitofwork.LMSBusinessUnitRepository.Get(x => x.StatusCode == StatusCodeConstants.Active).OrderBy(j => j.BusinessUnitName);               
                List<SelectListItem> ddBusinessUnits = new List<SelectListItem>();
                ddBusinessUnits.Add(defaultItem); 
               businessUnits.ToList().ForEach(x => ddBusinessUnits.Add(new SelectListItem { Text = x.BusinessUnitName, Value = x.LMSBusinessUnitId.ToString() }));                
                ViewBag.ddBusinessUnits = ddBusinessUnits;

                // for initial orientation courses only
                //IEnumerable<LMSCourse> jobCourses = unitofwork.LMSCourseRepository.Get(x => x.StatusCode == StatusCodeConstants.Active && x.IsInitialOrientation == true);

                // sort and store list of Courses in  ViewBag.ddCourses to access from front end

                IEnumerable<LMSCourse> jobCourses = unitofwork.LMSCourseRepository.Get(x => x.StatusCode == StatusCodeConstants.Active).OrderBy(j => j.CourseName);
                List<SelectListItem> ddCourses = new List<SelectListItem>();
                ddCourses.Add(defaultItem);
                jobCourses.ToList().ForEach(x => ddCourses.Add(new SelectListItem { Text = x.CourseName, Value = x.LMSCourseId.ToString() }));

                ViewBag.ddCourses = ddCourses;               

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        #endregion

        #region AuditTable

        public ActionResult Audit()
        {
            return View();
        }
        #endregion

        #endregion
    }
}