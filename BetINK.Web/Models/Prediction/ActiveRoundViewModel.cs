namespace BetINK.Web.Models.Prediction
{
    using System.Collections.Generic;

    public class ActiveRoundViewModel
    {
        public int RoundNumber { get; set; }
        public bool AlreadyPredicted { get; set; }
        public List<MatchViewModel> StartedMatches { get; set; }
        public List<MatchViewModel> Matches { get; set; }
    }
}
