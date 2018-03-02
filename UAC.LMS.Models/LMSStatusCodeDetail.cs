using System;
using System.ComponentModel.DataAnnotations;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Status Code Constants.
    /// </summary>
    public class LMSStatusCodeDetail : IModel
    {
        [Key]
        [StringLength(DataLengthConstant.LENGTH_CODE)]
        public string StatusCodeId { get; set; }

        [Timestamp]
        public Byte[] RowVersion { get; set; }

        [Required]
        [StringLength(DataLengthConstant.LENGTH_DESCRIPTION)]
        public string StatusCodeName { get; set; }

        [StringLength(DataLengthConstant.LENGTH_CODE)]
        public string StatusCode { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
