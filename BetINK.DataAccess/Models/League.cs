namespace BetINK.DataAccess.Models
{
    using BetINK.Common.Constants;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class League
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxLength)]
        public string Country { get; set; }

        [MaxLength(DataConstants.UrlMaxLength)]
        public string EmblemUrl { get; set; }

        public List<FootballLeague> Teams { get; set; } = new List<FootballLeague>();

        public List<Match> Matches { get; set; } = new List<Match>();
    }
}
