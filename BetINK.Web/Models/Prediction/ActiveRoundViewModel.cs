namespace BetINK.Web.Models.Prediction
{
    using BetINK.Services.Models.Prediction;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ActiveRoundViewModel
    {
        public int RoundNumber { get; set; }
        public bool AlreadyPredicted { get; set; }
        public List<MatchViewModel> StartedMatches { get; set; }
        public List<MatchViewModel> Matches { get; set; }
    }
}
