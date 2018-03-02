using System.ComponentModel.DataAnnotations;

namespace UAC.LMS.Web.ViewModel
{
    // comment here 

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "FirstName required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "UserName required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword required")]
        public string ConfirmPassword { get; set; }
    }
}