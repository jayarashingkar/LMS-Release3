using System;

namespace UAC.LMS.Web.ViewModel
{
    /// <summary>
    /// View Model for Course History Grid
    /// </summary>
    public class LMSCourseHistory_VM
    {
        public int LMSCourseHistoryId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string CompletedDateInFormat
        {
            get { return (CompletedDate.HasValue) ? CompletedDate.Value.ToString("MM/dd/yyyy") : ""; }
        }
    }
}