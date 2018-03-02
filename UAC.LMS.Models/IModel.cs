using System;
using System.ComponentModel.DataAnnotations;

namespace UAC.LMS.Models
{
    /// <summary>
    /// It has common fields and all entities should inherit this model
    /// </summary>
    public interface IModel
    {
        int? CreatedBy { get; set; }

        DateTime? CreatedOn { get; set; }

        int? LastModifiedBy { get; set; }

        DateTime? LastModifiedOn { get; set; }

        [Timestamp]
        byte[] RowVersion { get; set; }

        [StringLength(DataLengthConstant.LENGTH_CODE)]
        string StatusCode { get; set; }
    }
}
