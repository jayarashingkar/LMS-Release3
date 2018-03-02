using System;

namespace UAC.LMS.Web.ViewModel
{
    /// <summary>
    /// View Model for Employee Course Grid
    /// </summary>
    public class LMSEmployeeCourse_VM
    {
        public int LMSEmployeeCourseId { get; set; }
        public int LMSEmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int LMSCourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Evaluation { get; set; }
        public string CourseLength { get; set; }
        public int Frequency { get; set; }
        public bool IsInitialOrientation { get; set; }
        public string Remarks { get; set; }
        public string InstructorName { get; set; }
        public string Status { get; set; }
        public DateTime? PreviousDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string PreviousDateInFormat
        {
            get { return (PreviousDate.HasValue) ? PreviousDate.Value.ToString("MM/dd/yyyy") : ""; }
        }
        public string CompletedDateInFormat
        {
            get { return (CompletedDate.HasValue) ? CompletedDate.Value.ToString("MM/dd/yyyy") : ""; }
        }
        public string DueDateInFormat
        {
            get { return (DueDate.HasValue) ? DueDate.Value.ToString("MM/dd/yyyy") : ""; }
        }

        public string BusinessUnitName { get; set; }
        public int JobTitleId { get; set; }
        public string JobTitleName { get; set; }
        public string Shift { get; set; }
        public string DepartmentName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string EmployeeNo { get; set; }
        public DateTime? HireDate { get; set; }
        public string StrHireDate
        {
            get { return (HireDate.HasValue) ? HireDate.Value.ToString("MM/dd/yyyy") : ""; }
        }
    }
}