namespace BetINK.DataAccess.Models
{
    using BetINK.Common.Constants;
    using BetINK.Common.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Match
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxLength)]
        public string HomeTeam { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxLength)]
        public string AwayTeam { get; set; }

        public ResultEnum? Result { get; set; }

        [Range(DataConstants.MinPoints, DataConstants.MaxPoints)]
        public decimal HomeWinPoints { get; set; }

        [Range(DataConstants.MinPoints, DataConstants.MaxPoints)]
        public decimal AwayWinPoints { get; set; }

        [Range(DataConstants.MinPoints, DataConstants.MaxPoints)]
        public decimal DrawPoints { get; set; }

        public DateTime MatchStart { get; set; }

        public int RoundId { get; set; }

        public Round Round { get; set; }

        public int? LeagueId { get; set; }

        public League League { get; set; }

        public List<Prediction> Predictions { get; set; } = new List<Prediction>();
    }
}
