namespace BetINK.Web.Areas.Admin.Models.Team
{
    using BetINK.Common.Constants;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TeamFormViewModel
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

        public List<int> LeagueIds { get; set; } = new List<int>();

        public List<SelectListItem> Leagues { get; set; }
    }
}
