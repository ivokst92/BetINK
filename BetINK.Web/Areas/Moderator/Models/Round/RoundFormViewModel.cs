namespace BetINK.Web.Areas.Moderator.Models.Round
{
    using BetINK.Common.Constants;
    using System.ComponentModel.DataAnnotations;

    public class RoundFormViewModel
    {
        [MaxLength(DataConstants.DescriptionMaxLength)]
        public string Description { get; set; }
        
        [Range(1, 100)]
        public int Number { get; set; }
    }
}
