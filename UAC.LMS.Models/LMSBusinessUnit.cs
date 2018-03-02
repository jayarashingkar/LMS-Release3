using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Entities for Business Units/Location
    /// </summary>
    public class LMSBusinessUnit : CustomField, IModel
    {
        public int LMSBusinessUnitId { get; set; }
        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string BusinessUnitName { get; set; }
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
