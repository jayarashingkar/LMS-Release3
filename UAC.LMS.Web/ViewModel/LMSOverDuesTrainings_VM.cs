using System;

namespace UAC.LMS.Web.ViewModel
{
    public class LMSOverDuesTrainings_VM
    {
        //public int LMSEmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNo { get; set; }
        public string TrainingEvent { get; set; }
        public int Frequency { get; set; }
        public string CourseNo { get; set; }
        public string Department { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string CompletedDateInFormat
        {
            get { return (CompletedDate.HasValue) ? CompletedDate.Value.ToString("MM/dd/yyyy") : ""; }
        }
        public DateTime? DueDate { get; set; }
        public string DueDateInFormat
        {
            get { return (DueDate.HasValue) ? DueDate.Value.ToString("MM/dd/yyyy") : ""; }
        }
        public int DaysRemain
        {
            get { return Convert.ToInt32((DueDate.HasValue) ? (DueDate - DateTime.Now).Value.TotalDays : 0); }
        }
    }

    public class LMSOverDuesTrainings_VM_Excel
    {
        public string Employee { get; set; }
        public string EmployeeNo { get; set; }
        public string TrainingEvent { get; set; }
        public int Frq { get; set; }
        public string CourseNo { get; set; }
        public string Department { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateDue { get; set; }
        public int DaysRemain { get; set; }
    }
}