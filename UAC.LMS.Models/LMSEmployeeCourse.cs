using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Entities to store employee course details.
    /// </summary>
    public class LMSEmployeeCourse : CustomField, IModel
    {
        public int LMSEmployeeCourseId { get; set; }

        public LMSEmployee LMSEmployee { get; set; }
        public int LMSEmployeeId { get; set; }

        public LMSCourse LMSCourse { get; set; }
        public int LMSCourseId { get; set; }

        [StringLength(DataLengthConstant.LENGTH_DOUBLE_NAME)]
        public string Evaluation { get; set; }
        [StringLength(DataLengthConstant.LENGTH_NOTES)]
        public string Remarks { get; set; }
        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string InstructorName { get; set; }
        public DateTime? PreviousDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? DueDate { get; set; }
        [NotMapped]
        public int Frequency { get; set; }
        #region IModel

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }
        [StringLength(DataLengthConstant.LENGTH_CODE)]
        public string StatusCode { get; set; }
        [ForeignKey("StatusCode")]
        public virtual LMSStatusCodeDetail StatusCodeDetail { get; set; }

        #endregion
    }
}
