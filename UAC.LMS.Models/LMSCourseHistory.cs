using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Entities to store Course History details
    /// </summary>
    public class LMSCourseHistory : CustomField, IModel
    {
        public int LMSCourseHistoryId { get; set; }

        public int LMSCourseId { get; set; }
        public LMSCourse LMSCourse { get; set; }

        public DateTime? CompletedDate { get; set; }

        public LMSEmployee LMSEmployee { get; set; }
        public int LMSEmployeeId { get; set; }

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