using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UAC.LMS.Web.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "* User name required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "* Password required")]
        public string Password { get; set; }
    }
}

