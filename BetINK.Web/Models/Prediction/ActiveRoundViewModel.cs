namespace BetINK.Web.Models.Prediction
{
    using BetINK.Services.Models.Prediction;
    using System.Collections.Generic;

    public class ActiveRoundViewModel
    {
        public int RoundNumber { get; set; }
        public bool AlreadyPredicted { get; set; }
        public List<MatchServiceModel> Matches { get; set; }
    }
}
