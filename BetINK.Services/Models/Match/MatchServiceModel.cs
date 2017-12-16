namespace BetINK.Services.Models.Match
{
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;
    using System;

    public class MatchServiceModel : IMapFrom<Match>
    {
        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public decimal HomeWinPoints { get; set; }

        public decimal AwayWinPoints { get; set; }

        public decimal DrawPoints { get; set; }

        public DateTime MatchStart { get; set; }

        public int RoundId { get; set; }

        public int? LeagueId { get; set; }
    }
}
