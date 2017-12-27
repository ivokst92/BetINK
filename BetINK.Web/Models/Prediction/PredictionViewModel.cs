namespace BetINK.Web.Models.Prediction
{
    using System.Collections.Generic;

    public class PredictionViewModel
    {
        public int RoundNumber { get; set; }
        public List<MatchViewModel> Matches { get; set; }
    }
}
