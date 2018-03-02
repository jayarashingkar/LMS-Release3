using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Security Answers for Login Users
    /// </summary>
    public class LMSUserSecurityAnswer : CustomField, IModel
    {
        public int LMSUserSecurityAnswerId { get; set; }

        public int LMSLoginId { get; set; }
        public LMSLogin LMSLogin { get; set; }

        public int LMSSecurityQuestionId { get; set; }
        public LMSSecurityQuestion LMSSecurityQuestion { get; set; }

        [StringLength(DataLengthConstant.LENGTH_DESCRIPTION)]
        public string SecurityAnswer { get; set; }

        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        public bool HasQuestionId { get; set; }

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
