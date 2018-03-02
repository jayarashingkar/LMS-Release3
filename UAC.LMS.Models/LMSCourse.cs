using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Entities to store course details
    /// </summary>
    public class LMSCourse : CustomField, IModel
    {
        public int LMSCourseId { get; set; }
        [StringLength(DataLengthConstant.LENGTH_DOUBLE_NAME)]
        public string CourseName { get; set; }
        [StringLength(DataLengthConstant.LENGTH_CODE)]
        public string CourseCode { get; set; }
        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string CourseLength { get; set; }
        public bool IsReocurring { get; set; }
        public int Frequency { get; set; }
        public bool IsInitialOrientation { get; set; }

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
