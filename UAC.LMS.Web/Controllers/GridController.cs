using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UAC.LMS.Common.Constants;
using UAC.LMS.DAL;
using UAC.LMS.Models;
using UAC.LMS.Web.Models;
using UAC.LMS.Web.ViewModel;

namespace UAC.LMS.Web.Controllers
{
    /// <summary>
    /// It has grid with search info.
    /// </summary>
    public class GridController : BaseController
    {
        public ActionResult GetBusinessUnits(DataGridoption option)
        {
            ///<summary>
            /// Returns in Json file, list of all the business units sorted in ascending or descending order by Business Unit Id or Name 
            ///</summary>            
            LMSDBContext context = null;
            List<LMSBusinessUnit> lstEntity = null;
            int total = 0;
            IQueryable<LMSBusinessUnit> query = null;
            try
            {
                context = new LMSDBContext();
                query = context.LMSBusinessUnit;

                if (!string.IsNullOrEmpty(option.searchBy) && !option.searchBy.Equals(""))
                {
                    query = from p in query
                            where (
                                p.BusinessUnitName.ToLower().Contains(option.searchBy)
                                || p.LMSBusinessUnitId.ToString().ToLower().Contains(option.searchBy)
                            )
                            select p;
                }
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    if (option.sortBy == "LMSBusinessUnitId")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.LMSBusinessUnitId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.LMSBusinessUnitId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "BusinessUnitName")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.BusinessUnitName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.BusinessUnitName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else
                    {
                        lstEntity = query.ToList();
                    }
                }
                else
                {
                    lstEntity = query.OrderByDescending(x => x.LMSBusinessUnitId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                }
                total = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstEntity, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCourses(DataGridoption option)
        {
            ///<summary>
            /// Returns in Json file, list of all the courses sorted in ascending or descending order by option 
            ///</summary>    

            LMSDBContext context = null;
            List<LMSCourse> lstEntity = null;
            int total = 0;
            IQueryable<LMSCourse> query = null;
            try
            {
                context = new LMSDBContext();
                query = context.LMSCourses;

                if (!string.IsNullOrEmpty(option.searchBy) && !option.searchBy.Equals(""))
                {
                    string[] searchSplit = option.searchBy.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (searchSplit != null && searchSplit.Length > 0)
                    {
                        foreach (var item in searchSplit)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var itemSplit = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (itemSplit != null && itemSplit.Length == 2)
                                {
                                    var value = itemSplit[1];
                                    if (!string.IsNullOrEmpty(value) && value.Trim() != "")
                                    {

                                        if (itemSplit[0] == "searchCourseName")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.CourseName.ToLower().Contains(value.Trim())

                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "searchCourseCode")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.CourseCode.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "CourseType")
                                        {
                                            if (value.Trim() == "IsReocurring")
                                            {
                                                query = from p in query
                                                        where (
                                                            p.IsReocurring.Equals(true)
                                                        )
                                                        select p;
                                            }
                                            else if (value.Trim() == "IsInitialOrientation")
                                            {
                                                query = from p in query
                                                        where (
                                                            p.IsInitialOrientation.Equals(true)
                                                        )
                                                        select p;
                                            }
                                            else if (value.Trim() == "IsAll")
                                            {
                                                query = from p in query
                                                        where (
                                                            p.IsReocurring.Equals(true) || p.IsReocurring.Equals(false)
                                                            || p.IsInitialOrientation.Equals(true) || p.IsInitialOrientation.Equals(false)
                                                        )
                                                        select p;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    if (option.sortBy == "CourseName")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.CourseName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.CourseName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "CourseCode")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.CourseCode).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.CourseCode).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "CourseLength")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.CourseLength).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.CourseLength).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "Frequency")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.Frequency).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.Frequency).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "IsInitialOrientation")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.IsInitialOrientation).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.IsInitialOrientation).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "IsReocurring")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.IsReocurring).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.IsReocurring).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else
                    {
                        lstEntity = query.ToList();
                    }
                }
                else
                {
                    lstEntity = query.OrderByDescending(x => x.LMSCourseId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                }
                total = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstEntity, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDepartments(DataGridoption option)
        {
            ///<summary>
            /// Returns in Json file, list of all the departments sorted in ascending or descending order by option 
            ///</summary>   
            LMSDBContext context = null;
            List<LMSDepartment> lstEntity = null;
            int total = 0;
            IQueryable<LMSDepartment> query = null;
            try
            {
                context = new LMSDBContext();
                query = context.LMSDepartments;

                if (!string.IsNullOrEmpty(option.searchBy) && !option.searchBy.Equals(""))
                {

                    query = from p in query
                            where (
                                p.DepartmentCode.ToLower().Contains(option.searchBy)
                                || p.DepartmentName.ToString().ToLower().Contains(option.searchBy)
                                || p.LMSDepartmentId.ToString().ToLower().Contains(option.searchBy)
                            //|| p.LMSBusinessUnitId.ToString().ToLower().Contains(option.searchBy)
                            )
                            select p;
                }
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    if (option.sortBy == "DepartmentCode")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.DepartmentCode).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.DepartmentCode).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "DepartmentName")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.DepartmentName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.DepartmentName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "LMSDepartmentId")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.LMSDepartmentId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.LMSDepartmentId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else
                    {
                        lstEntity = query.ToList();
                    }
                }
                else
                {
                    lstEntity = query.OrderByDescending(x => x.LMSDepartmentId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                }
                total = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstEntity, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployees(DataGridoption option)
        {
            ///<summary>
            /// Returns in Json file, list of all the employees sorted in ascending or descending order by option 
            ///</summary>   
            LMSDBContext context = null;
            List<LMSEmployee> lstEntity = null;
            int total = 0;
            IQueryable<LMSEmployee> query = null;
            try
            {
                context = new LMSDBContext();
                query = context.LMSEmployees.Include("LMSBusinessUnit").Where(x => x.StatusCode != StatusCodeConstants.InActive);

                if (!string.IsNullOrEmpty(option.searchBy) && !option.searchBy.Equals(""))
                {
                    string[] searchSplit = option.searchBy.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (searchSplit != null && searchSplit.Length > 0)
                    {
                        foreach (var item in searchSplit)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var itemSplit = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (itemSplit != null && itemSplit.Length == 2)
                                {
                                    var value = itemSplit[1];
                                    if (!string.IsNullOrEmpty(value) && value.Trim() != "")
                                    {
                                        if (itemSplit[0] == "FirstName")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.FirstName.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "LastName")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.LastName.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "EmployeeNo")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.EmployeeNo.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "StatusCode")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.StatusCode.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "LMSBusinessUnitId")
                                        {
                                            int LMSBusinessUnitId = Convert.ToInt32(value);
                                            query = from p in query
                                                    where (
                                                        p.LMSBusinessUnitId == LMSBusinessUnitId
                                                    )
                                                    select p;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    if (option.sortBy == "FirstName")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.FirstName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.FirstName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "LastName")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.LastName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.LastName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "LMSEmployeeId")
                    {
                        //if (option.sortDirection == "asc")
                        //    lstEntity = query.OrderBy(x => x.EmployeeNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        //else
                        //    lstEntity = query.OrderByDescending(x => x.EmployeeNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();

                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.LMSEmployeeId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.LMSEmployeeId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();

                    }
                    else if (option.sortBy == "EmployeeNo")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.EmployeeNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.EmployeeNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();

                        //if (option.sortDirection == "asc")
                        //    lstEntity = query.OrderBy(x => x.LMSEmployeeId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        //else
                        //    lstEntity = query.OrderByDescending(x => x.LMSEmployeeId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();

                    }
                    else if (option.sortBy == "StatusCode")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.StatusCode).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.StatusCode).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "MiddleName")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.MiddleName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.MiddleName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "HireDate")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.HireDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.HireDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "Shift")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.Shift).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.Shift).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else
                    {
                        lstEntity = query.ToList(); ;
                       
                    }
                }
                else
                {
                    lstEntity = query.OrderByDescending(x => x.LMSEmployeeId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                }
                total = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstEntity, total = total }, JsonRequestBehavior.AllowGet);
        }

        //start here 


        public ActionResult GetEmployeeCourses(DataGridoption option)
        {
            ///<summary>
            /// Returns in Json file, list of all the courses taken by employee sorted in ascending or descending order by option 
            ///</summary>   
            LMSDBContext context = null;
            List<LMSEmployeeCourse_VM> lstEntity = null;
            int total = 0;
            IQueryable<LMSEmployeeCourse> query = null;
            IQueryable<LMSEmployeeCourse> query1 = null;
            try
            {
                context = new LMSDBContext();
                query = context.LMSEmployeeCourses.Include("LMSEmployee").Include("LMSCourse").Where(x => x.StatusCode != StatusCodeConstants.InActive);

                if (!string.IsNullOrEmpty(option.searchBy) && !option.searchBy.Equals(""))
                {
                    string[] searchSplit = option.searchBy.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (searchSplit != null && searchSplit.Length > 0)
                    {
                        foreach (var item in searchSplit)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var itemSplit = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (itemSplit != null && itemSplit.Length == 2)
                                {
                                    var value = itemSplit[1];
                                    if (!string.IsNullOrEmpty(value) && value.Trim() != "")
                                    {

                                        if (itemSplit[0] == "LMSEmployeeId")
                                        {
                                            int eId = Convert.ToInt32(value);
                                            query = from p in query
                                                    where (
                                                        //p.LMSEmployeeId.ToString().ToLower().Contains(value.Trim())
                                                        p.LMSEmployeeId == eId
                                                    )
                                                    select p;
                                            //query = from p in query
                                            //        where (
                                            //            p.LMSEmployeeId.ToString().ToLower().Contains(value.Trim())
                                            //        )
                                            //        select p;
                                        }
                                        if (itemSplit[0] == "searchCourseName")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.LMSCourse.CourseName.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "searchCourseCode")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.LMSCourse.CourseCode.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "CourseType")
                                        {
                                            if (value.Trim() == "IsReocurring")
                                            {
                                                query = from p in query
                                                        where (
                                                            p.LMSCourse.IsReocurring.Equals(true)
                                                        )
                                                        select p;
                                            }
                                            else if (value.Trim() == "IsInitialOrientation")
                                            {
                                                query = from p in query
                                                        where (
                                                            p.LMSCourse.IsInitialOrientation.Equals(true)
                                                        )
                                                        select p;
                                            }
                                            else if (value.Trim() == "IsAll")
                                            {
                                                query = from p in query
                                                        where (
                                                            p.LMSCourse.IsReocurring.Equals(true) ||
                                                            p.LMSCourse.IsInitialOrientation.Equals(true)
                                                        )
                                                        select p;
                                            }

                                        }
                                        if (itemSplit[0] == "searchLMSJobTitleId")
                                        {
                                            if (value.Trim() != "-1")
                                            {
                                                List<int> titleid = new List<int>();
                                                string[] arr = value.Trim().Split(',');
                                                if (arr != null)
                                                {
                                                    foreach (var tId in arr)
                                                        if (tId != "-1")
                                                            titleid.Add(Convert.ToInt32(tId));

                                                    var courseIds = context.LMSJobTitleCourses.Where(x =>
                                                                    titleid.Contains(x.LMSJobTitleId)).Select(x => x.LMSCourseId).ToList();
                                                    if (courseIds != null)
                                                    {
                                                        query = from p in query
                                                                where (
                                                                    courseIds.Contains(p.LMSCourseId)
                                                                )
                                                                select p;
                                                    }
                                                }
                                            }
                                        }
                                        if (itemSplit[0] == "IsJobTitle")
                                        {
                                            if (value.Trim() != "-1")
                                            {
                                                var tId = Convert.ToInt32(value);
                                                var courseIds = context.LMSJobTitleCourses.Where(x =>
                                                                    x.LMSJobTitleId == tId).Select(x => x.LMSCourseId).ToList();
                                                if (courseIds != null)
                                                {
                                                    query = from p in query
                                                            where (
                                                                courseIds.Contains(p.LMSCourseId)
                                                            )
                                                            select p;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                query1 = query;
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    // query1 = query;
                    if (option.sortBy == "InstructorName")
                    {
                        if (option.sortDirection == "asc")

                            query = query.OrderBy(x => x.InstructorName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);

                        else
                            query = query.OrderByDescending(x => x.InstructorName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                    else if (option.sortBy == "CourseName")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.LMSCourse.CourseName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.LMSCourse.CourseName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                    //adding here Course Code
                    else if (option.sortBy == "CourseCode")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.LMSCourse.CourseCode).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.LMSCourse.CourseCode).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                    else if (option.sortBy == "Frequency")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.LMSCourse.Frequency).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.LMSCourse.Frequency).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                    else if (option.sortBy == "IsInitialOrientation")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.LMSCourse.IsInitialOrientation).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.LMSCourse.IsInitialOrientation).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                    else if (option.sortBy == "CourseLength")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.LMSCourse.CourseLength).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.LMSCourse.CourseLength).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                    // Removed Previous Date 11/21/2016
                    //comment
                    else if (option.sortBy == "PreviousDateInFormat")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.PreviousDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.PreviousDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                    //till here
                    else if (option.sortBy == "DueDateInFormat")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.DueDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.DueDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                    else if (option.sortBy == "CompletedDateInFormat")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.CompletedDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.CompletedDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                }
                else
                {
                    //  query1 = query;
                    query = query.OrderByDescending(x => x.LMSEmployeeCourseId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    //  query1 = query;
                }

                lstEntity = query
                .Select(x => new LMSEmployeeCourse_VM
                {
                    LMSEmployeeCourseId = x.LMSEmployeeCourseId,
                    LMSEmployeeId = x.LMSEmployeeId,
                    EmployeeName = x.LMSEmployee.FirstName + " " + x.LMSEmployee.LastName,
                    LMSCourseId = x.LMSCourseId,
                    CourseName = x.LMSCourse.CourseName,
                    //adding here Course Code
                    CourseCode = x.LMSCourse.CourseCode,
                    CourseLength = x.LMSCourse.CourseLength,
                    Frequency = x.LMSCourse.Frequency,
                    IsInitialOrientation = x.LMSCourse.IsInitialOrientation,
                    InstructorName = x.InstructorName,
                    Evaluation = x.Evaluation,
                    Remarks = x.Remarks,
                    // Removed Previous Date 11/21/2016
                    //PreviousDate = x.PreviousDate,
                    PreviousDate = x.CompletedDate,
                    CompletedDate = x.CompletedDate,
                    DueDate = x.DueDate,
                }).ToList();

                if (query1 != null)
                    total = query1.Count();
                //    total = query.Count();
               // total = lstEntity.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstEntity, total = total }, JsonRequestBehavior.AllowGet);
        }
        //ends here 

        public ActionResult GetJobTitles(DataGridoption option)
        {
            ///<summary>
            /// Returns in Json file, list of all the job titles sorted in ascending or descending order by option 
            ///</summary>   
            LMSDBContext context = null;
            List<LMSJobTitle> lstEntity = null;
            int total = 0;
            IQueryable<LMSJobTitle> query = null;
            try
            {
                context = new LMSDBContext();
                query = context.LMSJobTitles;

                if (!string.IsNullOrEmpty(option.searchBy) && !option.searchBy.Equals(""))
                {
                    query = from p in query
                            where (
                                p.JobTitleName.ToLower().Contains(option.searchBy)
                                || p.LMSJobTitleId.ToString().ToLower().Contains(option.searchBy)
                            )
                            select p;
                }
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    if (option.sortBy == "LMSJobTitleId")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.LMSJobTitleId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.LMSJobTitleId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "JobTitleName")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.JobTitleName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.JobTitleName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else
                    {
                        lstEntity = query.ToList();
                    }
                }
                else
                {
                    lstEntity = query.OrderByDescending(x => x.LMSJobTitleId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                }
                total = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstEntity, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserLogins(DataGridoption option)
        {
            ///<summary>
            /// Returns in Json file, list of all the User Logins sorted in ascending or descending order by option 
            ///</summary>   
            LMSDBContext context = null;
            List<LMSLogin> lstEntity = null;
            int total = 0;
            IQueryable<LMSLogin> query = null;
            try
            {
                context = new LMSDBContext();
                //changin here //adding here 12/22/2016
                //  query = context.LMSLogins.Where(x => x.UserType != UserTypeConstants.SuperAdmin);
                query = context.LMSLogins.Where(x => x.UserType != UserTypeConstants.SuperAdmin);

                if (!string.IsNullOrEmpty(option.searchBy) && !option.searchBy.Equals(""))
                {
                    string[] searchSplit = option.searchBy.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (searchSplit != null && searchSplit.Length > 0)
                    {
                        foreach (var item in searchSplit)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var itemSplit = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (itemSplit != null && itemSplit.Length == 2)
                                {
                                    var value = itemSplit[1];
                                    if (!string.IsNullOrEmpty(value) && value.Trim() != "")
                                    {
                                        if (itemSplit[0] == "UserName")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.UserName.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "FirstName")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.FirstName.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "LastName")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.LastName.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        //commented here 11/14/2016
                                        //if (itemSplit[0] == "EmployeeNo")
                                        //{
                                        //    query = from p in query
                                        //            where (
                                        //                p.EmployeeNo.ToLower().Contains(value.Trim())
                                        //            )
                                        //            select p;
                                        //}
                                        if (itemSplit[0] == "StatusCode")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.StatusCode.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    if (option.sortBy == "UserName")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.UserName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.UserName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "FirstName")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.FirstName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.FirstName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "LastName")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.LastName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.LastName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    ////commented here 11/14/2016
                    //else if (option.sortBy == "EmployeeNo")
                    //{
                    //    if (option.sortDirection == "asc")
                    //        lstEntity = query.OrderBy(x => x.EmployeeNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    //    else
                    //        lstEntity = query.OrderByDescending(x => x.EmployeeNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    //}
                    else
                    {
                        lstEntity = query.ToList();
                    }
                }
                else
                {
                    lstEntity = query.OrderByDescending(x => x.UserId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                }
                total = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstEntity, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPrintRosters(DataGridoption option)
        {
            // comment here 

            LMSDBContext context = null;
            List<LMSEmployeeCourse_VM> lstEntity = null;
            int total = 0;
            IQueryable<LMSEmployeeCourse> query = null;
            try
            {
                context = new LMSDBContext();
                query = context.LMSEmployeeCourses.Include("LMSEmployee").Include("LMSCourse").Where(x => x.StatusCode != StatusCodeConstants.InActive);
                lstEntity = query
                .Select(x => new LMSEmployeeCourse_VM
                {
                    EmployeeName = x.LMSEmployee.FirstName + " " + x.LMSEmployee.LastName,
                    DepartmentName = x.LMSEmployee.LMSDepartment.DepartmentName,

                }).ToList();
                total = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstEntity, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAudit(DataGridoption option)
        {
            // comment here 

            ///<summary>
            /// Returns in Json file, list of all the Audit table entires sorted in ascending or descending order Audit Id 
            ///</summary>   

            LMSDBContext context = null;
            List<LMSAudit> lstEntity = null;
            int total = 0;
            IQueryable<LMSAudit> query = null;
            try
            {
                context = new LMSDBContext();
                query = context.LMSAudits;

                if (!string.IsNullOrEmpty(option.searchBy) && !option.searchBy.Equals(""))
                {
                    string[] searchSplit = option.searchBy.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (searchSplit != null && searchSplit.Length > 0)
                    {
                        foreach (var item in searchSplit)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var itemSplit = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (itemSplit != null && itemSplit.Length == 2)
                                {
                                    var value = itemSplit[1];
                                    if (!string.IsNullOrEmpty(value) && value.Trim() != "")
                                    {
                                        if (itemSplit[0] == "searchUserName")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.UserName.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "searchFullName")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.FullName.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "searchDesc")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.Description.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "searchDate" && !string.IsNullOrEmpty(value) && value.Trim() != "")
                                        {
                                            DateTime tdate = Convert.ToDateTime(value);
                                            query = from p in query
                                                    where (
                                                     DbFunctions.TruncateTime(p.TransactionDate) == DbFunctions.TruncateTime(tdate)
                                                    )
                                                    select p;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    if (option.sortBy == "LMSAuditId")
                    {
                        if (option.sortDirection == "asc")
                            lstEntity = query.OrderBy(x => x.LMSAuditId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            lstEntity = query.OrderByDescending(x => x.LMSAuditId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }

                    else
                    {
                        lstEntity = query.ToList();
                    }
                }
                else
                {
                    lstEntity = query.OrderByDescending(x => x.LMSAuditId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                }
                total = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstEntity, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpCourseHistory(DataGridoption option)
        {
            // comment here 

            ///<summary>
            /// Returns in Json file, list of all the Course History table entires sorted in ascending or descending order Audit Id 
            ///</summary>   

            LMSDBContext context = null;
            List<LMSCourseHistory_VM> lstEntity = null;
            int total = 0;
            IQueryable<LMSCourseHistory> query = null;
            IQueryable<LMSCourseHistory> query1 = null;

            try
            {
                context = new LMSDBContext();
                query = context.LMSCourseHistories;

                if (!string.IsNullOrEmpty(option.searchBy) && !option.searchBy.Equals(""))
                {
                    string[] searchSplit = option.searchBy.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (searchSplit != null && searchSplit.Length > 0)
                    {
                        foreach (var item in searchSplit)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var itemSplit = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (itemSplit != null && itemSplit.Length == 2)
                                {
                                    var value = itemSplit[1];
                                    if (!string.IsNullOrEmpty(value) && value.Trim() != "")
                                    {
                                        if (itemSplit[0] == "hdnEmployeeId")
                                        {
                                            int empId = Convert.ToInt32(value);
                                            query = from p in query
                                                    where (
                                                        p.LMSEmployeeId == empId
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "LMSCourseName" && value != "-1")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.LMSCourse.CourseName.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "LMSCourseCode" && value != "-1")
                                        {
                                            query = from p in query
                                                    where (
                                                        p.LMSCourse.CourseCode.ToLower().Contains(value.Trim())
                                                    )
                                                    select p;
                                        }
                                        if (itemSplit[0] == "searchFromDate" && !string.IsNullOrEmpty(value) && value.Trim() != "")
                                        {
                                            DateTime tdate = Convert.ToDateTime(value);
                                            query = from p in query
                                                    where (
                                                     DbFunctions.TruncateTime(p.CompletedDate) >= DbFunctions.TruncateTime(tdate)
                                                    )
                                                    select p;
                                        }

                                        if (itemSplit[0] == "searchToDate" && !string.IsNullOrEmpty(value) && value.Trim() != "")
                                        {
                                            DateTime tdate = Convert.ToDateTime(value);
                                            query = from p in query
                                                    where (
                                                     DbFunctions.TruncateTime(p.CompletedDate) <= DbFunctions.TruncateTime(tdate)
                                                    )
                                                    select p;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                query1 = query;
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    if (option.sortBy == "CompletedDateInFormat")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.CompletedDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.CompletedDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                    else if (option.sortBy == "CourseName")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.LMSCourse.CourseName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.LMSCourse.CourseName).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                    else if (option.sortBy == "CourseCode")
                    {
                        if (option.sortDirection == "asc")
                            query = query.OrderBy(x => x.LMSCourse.CourseCode).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                        else
                            query = query.OrderByDescending(x => x.LMSCourse.CourseCode).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                    }
                }
                else
                {
                    query = query.OrderByDescending(x => x.LMSCourseHistoryId).Skip(option.pageSize * option.pageIndex).Take(option.pageSize);
                }
                if (query1 != null)
                    total = query1.Count();
                // total = query.Count();
                lstEntity = (from model in query
                             select new LMSCourseHistory_VM
                             {
                                 LMSCourseHistoryId = model.LMSCourseHistoryId,
                                 CourseName = model.LMSCourse.CourseName,
                                 CourseCode = model.LMSCourse.CourseCode,
                                 CompletedDate = model.CompletedDate
                             }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = lstEntity, total = total }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetTrainingOverDues(DataGridoption option)
        {
            LMSDBContext context = null;
            List<LMSOverDuesTrainings_VM> overdues = null;
            IQueryable<LMSOverDuesTrainings_VM> query = null;
            int total = 0;
            try
            {
                context = new LMSDBContext();
                query = (from emp in context.LMSEmployees.Where(j => j.StatusCode.ToLower() == "active")
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
                            //   });
                         }).OrderByDescending(j=>j.DueDate).Take(50);
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    if (option.sortBy == "EmployeeNo")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.EmployeeNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.EmployeeNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "TrainingEvent")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.TrainingEvent).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.TrainingEvent).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "Frequency")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.Frequency).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.Frequency).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "CourseNo")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.CourseNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.CourseNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "Department")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.Department).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.Department).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "CompletedDate")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.CompletedDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.CompletedDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "DueDate" || option.sortBy == "DaysRemain")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.DueDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.DueDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else
                    {
                        overdues = query.ToList();
                    }
                }
                else
                {
                    overdues = query.ToList();
                }
                total = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = overdues, total = total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTrainingnotTaken(DataGridoption option)
        {
            LMSDBContext context = null;
            List<LMSOverDuesTrainings_VM> overdues = null;
            IQueryable<LMSOverDuesTrainings_VM> query = null;
            int total = 0;
            try
            {
                context = new LMSDBContext();
                //query = (from emp in context.LMSEmployees
                //         join empcourse in context.LMSEmployeeCourses.Where(x => x.CompletedDate == null) on emp.LMSEmployeeId equals empcourse.LMSEmployeeId
                //         join course in context.LMSCourses on empcourse.LMSCourseId equals course.LMSCourseId
                //         select new LMSOverDuesTrainings_VM
                //         {
                //             //LMSEmployeeId = emp.LMSEmployeeId,
                //             EmployeeName = emp.FirstName + ", " + emp.LastName,
                //             EmployeeNo = emp.EmployeeNo,
                //             TrainingEvent = course.CourseName,
                //             Frequency = course.Frequency,
                //             CourseNo = course.CourseCode,
                //             Department = emp.LMSDepartment.DepartmentName,
                //             CompletedDate = empcourse.CompletedDate,
                //             DueDate = empcourse.DueDate,
                //         });
                query = (from emp in context.LMSEmployees.Where(j => j.StatusCode.ToLower() == "active")
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
                       //  });
                         }).OrderByDescending(j => j.DueDate).Take(50);
                if (!string.IsNullOrEmpty(option.sortBy))
                {
                    if (option.sortBy == "EmployeeNo")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.EmployeeNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.EmployeeNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "TrainingEvent")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.TrainingEvent).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.TrainingEvent).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "Frequency")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.Frequency).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.Frequency).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "CourseNo")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.CourseNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.CourseNo).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "Department")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.Department).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.Department).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "CompletedDate")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.CompletedDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.CompletedDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else if (option.sortBy == "DueDate" || option.sortBy == "DaysRemain")
                    {
                        if (option.sortDirection == "asc")
                            overdues = query.OrderBy(x => x.DueDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                        else
                            overdues = query.OrderByDescending(x => x.DueDate).Skip(option.pageSize * option.pageIndex).Take(option.pageSize).ToList();
                    }
                    else
                    {
                        overdues = query.ToList();
                    }
                }
                else
                {
                    overdues = query.ToList();
                }
                total = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { items = overdues, total = total }, JsonRequestBehavior.AllowGet);
        }
    }
}
