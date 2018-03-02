using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Security Questions for Login Users
    /// </summary>
    public class LMSSecurityQuestion : CustomField, IModel
    {
        public int LMSSecurityQuestionId { get; set; }

        [StringLength(DataLengthConstant.LENGTH_NOTES)]
        public string Question { get; set; }

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
