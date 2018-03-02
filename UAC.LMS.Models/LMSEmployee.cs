using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UAC.LMS.Models
{
    /// <summary>
    /// Entities to store employee informations.
    /// </summary>
    public class LMSEmployee : CustomField, IModel
    {
        public int LMSEmployeeId { get; set; }
        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string FirstName { get; set; }
        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string LastName { get; set; }
        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string MiddleName { get; set; }
        [StringLength(DataLengthConstant.LENGTH_NAME)]
        public string EmployeeNo { get; set; }
        public DateTime? HireDate { get; set; }
        [StringLength(DataLengthConstant.LENGTH_EMAIL)]
        public string Email { get; set; }

        public bool IsInitialOrientationApplied { get; set; }

        [NotMapped]
        public string StrHireDate
        {
            get { return (HireDate.HasValue) ? HireDate.Value.ToString("MM/dd/yyyy") : "-"; }
        }

        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }


        [StringLength(DataLengthConstant.LENGTH_CODE)]
        public string Shift { get; set; }

        public LMSDepartment LMSDepartment { get; set; }
        public int LMSDepartmentId { get; set; }

        [NotMapped]
        public string DepartmentName { get; set; }

        public LMSJobTitle LMSJobTitle { get; set; }
        public int LMSJobTitleId { get; set; }

        [NotMapped]
        public string JobTitleName { get; set; }

        public LMSBusinessUnit LMSBusinessUnit { get; set; }
        public int LMSBusinessUnitId { get; set; }

        [NotMapped]
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

        //Removed the comments after creating the database
        // Do not comment after

        [ForeignKey("StatusCode")]
        public virtual LMSStatusCodeDetail StatusCodeDetail { get; set; }

        // Use the code below After first time database creation
        // Comment - During first time database creation

        //[ForeignKey("StatusCode")] 
        //public virtual LMSStatusCodeDetail StatusCodeDetail { get; set; }

        #endregion
    }
}
