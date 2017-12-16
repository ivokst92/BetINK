namespace BetINK.Web.Areas.Admin.Models.League
{
    using BetINK.Common.Constants;
    using System.ComponentModel.DataAnnotations;

    public class LeagueFormViewModel
    {
        [Required]
        [MaxLength(DataConstants.MaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxLength)]
        public string Country { get; set; }

        [Display(Name = "Emblem Url")]
        [MaxLength(DataConstants.UrlMaxLength)]
        public string EmblemUrl { get; set; }
    }
}
