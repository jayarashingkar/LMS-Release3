using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAC.LMS.Web.Models
{
    // comment here 

    /// <summary>
    /// Setting the Grid paramaters to display data
    /// </summary>
    public class DataGridoption
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string sortDirection { get; set; }
        public string sortBy { get; set; }
        public string filterBy { get; set; }
        public string searchBy { get; set; }
    }
}