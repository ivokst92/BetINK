namespace BetINK.Web.Models.Prediction
{
    using System.Collections.Generic;

    public class ActiveRoundViewModel : PredictionViewModel
    {
        public bool AlreadyPredicted { get; set; }
        public List<MatchViewModel> StartedMatches { get; set; }
    }
}
