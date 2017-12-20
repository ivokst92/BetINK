namespace BetINK.Web.Models.ManageViewModels
{
    using BetINK.Common.Constants;
    using System.ComponentModel.DataAnnotations;

    public class IndexViewModel
    {
        public string Username { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MinLength(DataConstants.MinLength)]
        [MaxLength(DataConstants.MaxLength)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MinLength(DataConstants.MinLength)]
        [MaxLength(DataConstants.MaxLength)]
        public string LastName { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
