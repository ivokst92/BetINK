namespace BetINK.Web.Areas.Moderator.Models.Match
{
    using BetINK.Common.Constants;
    using BetINK.Common.Resources;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class MatchFormViewModel : IValidatableObject
    {
        [Required]
        [MaxLength(DataConstants.MaxLength)]
        [Display(Name = "Home Team")]
        public string HomeTeam { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxLength)]
        [Display(Name = "Away Team")]
        public string AwayTeam { get; set; }

        [Required]
        [Range(DataConstants.MinPoints, DataConstants.MaxPoints)]
        [Display(Name = "Home Win Points")]
        public decimal HomeWinPoints { get; set; }

        [Required]
        [Range(DataConstants.MinPoints, DataConstants.MaxPoints)]
        [Display(Name = "Away Win Points")]
        public decimal AwayWinPoints { get; set; }

        [Required]
        [Range(DataConstants.MinPoints, DataConstants.MaxPoints)]
        [Display(Name = "Draw Points")]
        public decimal DrawPoints { get; set; }

        [Required]
        [Display(Name = "Match Start")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy H:mm}")]
        public DateTime MatchStart { get; set; }

        public int RoundId { get; set; }

        public int? LeagueId { get; set; }

        public List<SelectListItem> Teams { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HomeTeam == AwayTeam)
            {
                yield return new ValidationResult(MessageResources.msgTeamDuplicate, new[] { nameof(AwayTeam) });
            }
        }
    }
}
