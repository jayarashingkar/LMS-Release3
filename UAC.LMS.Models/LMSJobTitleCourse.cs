using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Entities to store jobtitle courses.
    /// </summary>
    public class LMSJobTitleCourse : CustomField, IModel
    {
        public int LMSJobTitleCourseId { get; set; }

        public LMSJobTitle LMSJobTitle { get; set; }
        public int LMSJobTitleId { get; set; }

        public LMSCourse LMSCourse { get; set; }
        public int LMSCourseId { get; set; }

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
