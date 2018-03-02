namespace UAC.LMS.Models
{
    /// <summary>
    /// LoggedIn User Details
    /// </summary>
    public class CurrentUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PermissionLevel { get; set; }
        public string FullName { get; set; }
        public bool IsSecurityApplied { get; set; }
    }
}
