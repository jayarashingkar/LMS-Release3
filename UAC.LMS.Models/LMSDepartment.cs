using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Entities for Departments
    /// </summary>
    public class LMSDepartment : CustomField, IModel
    {
        public int LMSDepartmentId { get; set; }
        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string DepartmentName { get; set; }
        [StringLength(DataLengthConstant.LENGTH_CODE)]
        public string DepartmentCode { get; set; }

        //Foreign Key - Mapping with Business Unit Entity
        //public LMSBusinessUnit LMSBusinessUnit { get; set; }
        //public int LMSBusinessUnitId { get; set; }

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
