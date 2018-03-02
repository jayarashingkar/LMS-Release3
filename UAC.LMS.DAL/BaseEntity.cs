using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAC.LMS.DAL
{
    // comment here 

    /// <summary>
    /// Abstract class for common purpose
    /// </summary>
    public abstract class BaseEntity
    {
        internal Int64 CreateUser { get; set; }
        internal DateTime CreateDateTime { get; set; }
        internal DateTime ModDateTime;
        internal int ModUser;
    }
}
