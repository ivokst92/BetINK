namespace BetINK.DataAccess.Models
{
    using BetINK.Common.Constants;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class Team
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

        public List<FootballLeague> Leagues { get; set; } = new List<FootballLeague>();
    }
}
