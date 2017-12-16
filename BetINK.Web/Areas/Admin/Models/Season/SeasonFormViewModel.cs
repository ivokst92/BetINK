namespace BetINK.Web.Areas.Admin.Models.Season
{
    using BetINK.Common.Constants;
    using System.ComponentModel.DataAnnotations;

    public class SeasonFormViewModel
    {
        [MaxLength(DataConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxLength)]
        public string Name { get; set; }

        public bool HasActiveSeason { get; set; }
    }
}
