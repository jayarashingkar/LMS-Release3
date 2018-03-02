using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Enities auditing information
    /// </summary>
    public class LMSAudit : CustomField, IModel
    {
        //11/11/2016
        public int LMSAuditId { get; set; }

        //public int TransactionId { get; set; }

        public DateTime? TransactionDate { get; set; }

        [NotMapped]
        public string TransactionDateInFormat
        {
            get { return (TransactionDate.HasValue) ? TransactionDate.Value.ToString("MM/dd/yyyy, hh:mm:ss") : ""; }
        }

        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string UserName { get; set; }
        
            
        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string FullName { get; set; }

       // [StringLength(DataLengthConstant.LENGTH_NAME)]
     //   public string FirstName { get; set; }

     //   [StringLength(DataLengthConstant.LENGTH_NAME)]
     //   public string LastName { get; set; }

        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string Section { get; set; }

        [StringLength(DataLengthConstant.LENGTH_DESCRIPTION)]
        public string Action { get; set; }

        [StringLength(DataLengthConstant.LENGTH_NOTES)]
        public string Description { get; set; }

        ////  public int LoginId { get; set; }

        ////public int EmployeeId { get; set; }

        //[StringLength(DataLengthConstant.LENGTH_NAME)]
        //public string TransactionName { get; set; }

        //[StringLength(DataLengthConstant.LENGTH_DESCRIPTION)]
        //public string Description { get; set; }


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
