using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAC.LMS.Web.ViewModel
{
    public class ImportEmployee_VM
    {
        public int LMSEmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? HireDate { get; set; }
        public string Email { get; set; }
        public string Shift { get; set; }
        public int LMSDepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public int LMSJobTitleId { get; set; }
        public string JobTitleName { get; set; }
        public int LMSBusinessUnitId { get; set; }
        public string Status { get; set; }

    }
}