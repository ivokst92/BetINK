namespace BetINK.DataAccess.Models
{
    using BetINK.Common.Enums;
    using System;

    public class Prediction
    {
        public int MatchId { get; set; }

        public Match Match { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public ResultEnum MatchPrediction { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
