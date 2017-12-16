namespace BetINK.Web.Areas.Admin.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class UserRoleViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
